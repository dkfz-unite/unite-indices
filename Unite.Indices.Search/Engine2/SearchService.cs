using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Access;
using Unite.Indices.Search.Engine2.Converters;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Criteria.Properties;
using Unite.Indices.Search.Engine2.Fields;
using Unite.Indices.Search.Engine2.References;

namespace Unite.Indices.Search.Engine2;

/// <summary>
/// Stage of a search: a document with criteria, the entity joining it with the target and the entity keys it collects.
/// </summary>
/// <param name="Stage">Document stage.</param>
/// <param name="IsExclusion">The stage searches the document's exclude criteria: its keys are excluded from the target.</param>
/// <param name="Join">Key of the entity joining the document with the target.</param>
/// <param name="Collects">Keys collected by the stage with one request: the join key and the document's own identity key, if it is an entity.</param>
/// <param name="Bridge">Target entity the join keys are mapped to along the main tree, if the target does not refer to the join entity
/// (e.g. specimen keys of SMs mapped to donor keys for a project search); null otherwise.</param>
/// <param name="Rank">Execution rank derived from the entity trees (see <see cref="SearchTrees.GetRank"/>).</param>
public record SearchStage(IDocumentStage Stage, bool IsExclusion, DocumentKey Join, IReadOnlyList<DocumentKey> Collects, SearchEntity Bridge, (int Level, int Group, int Depth) Rank);

/// <summary>
/// Searches documents of type <typeparamref name="T"/> with criteria on any searchable documents.
/// Criteria of other documents are applied through the entities (donors, specimens, genes, proteins) they share with the target,
/// derived from <see cref="Unite.Indices.Entities.EntityKeyAttribute"/> and <see cref="Unite.Indices.Entities.ReferencesAttribute"/>.
/// </summary>
/// <remarks>
/// Each document has include and exclude criteria (<see cref="DocumentCriteria{TDocument}"/>): entity keys of documents matching the
/// include criteria are required, those of documents matching the exclude criteria are excluded from the target
/// (e.g. images with SMs in TP53, but without SMs in IDH1).
/// Searches of users are restricted to the data they have access to (<see cref="IAccessService"/>): the restriction runs as the
/// first stage, or directly in the target query if it is on the target itself.
/// </remarks>
public class SearchService<T> where T : class
{
    /// <summary>
    /// Page size of <see cref="Stats"/>.
    /// </summary>
    public const int StatsPageSize = 499;

    private static readonly MethodInfo _createValueCriteria = typeof(SearchService<T>).GetMethod(nameof(CreateValueCriteria), BindingFlags.NonPublic | BindingFlags.Static);
    private static readonly Func<T, object> _id = CreateGetter<object>("Id");
    private static readonly Func<T, DataIndex> _data = CreateGetter<DataIndex>("Data");

    private readonly IServiceProvider _serviceProvider;


    public SearchService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    /// <summary>
    /// Searches documents matching the criteria in the scope, sorted by the document's sort orders (<see cref="SearchResults{TDocument}.Orders"/>).
    /// </summary>
    /// <param name="term">Free text searched in the target's searchable text fields, optional.</param>
    /// <exception cref="ArgumentException">Criteria of a document can't be applied to the target or are invalid.</exception>
    public async Task<SearchResult<T>> Search(CriteriaContainer criteria, SearchScope scope, int from = 0, int size = 20, string term = null)
    {
        var targetCriteria = await CreateTargetCriteria(criteria, scope);

        if (targetCriteria == null)
            return new SearchResult<T>();

        var result = await Query(targetCriteria, from, size, term);

        await Annotate(result.Rows, scope);

        return result;
    }

    /// <summary>
    /// Document with the given id, null if it does not exist or is not in the scope.
    /// </summary>
    /// <exception cref="NotSupportedException">The document has no searchable "id".</exception>
    public async Task<T> Get(string id, SearchScope scope)
    {
        var field = SearchFields<T>.Find("id") as SearchField<T>
            ?? throw new NotSupportedException($"{typeof(T).Name} has no searchable id.");

        var valueType = Nullable.GetUnderlyingType(field.ValueType) ?? field.ValueType;
        var idCriteria = (PropertyCriteria)_createValueCriteria.MakeGenericMethod(valueType).Invoke(null, [field, id]);

        if (idCriteria == null)
            return null;

        var criteria = new CriteriaContainer();
        criteria.Set(new DocumentCriteria<T> { Include = new CriteriaGroup { Nodes = [idCriteria] } });

        var targetCriteria = await CreateTargetCriteria(criteria, scope);

        if (targetCriteria == null)
            return null;

        // Large properties (e.g. the specimens of a gene) are left out of single documents.
        var result = await Query(targetCriteria, 0, 1, null, SearchResults<T>.GetExclusions);

        await Annotate(result.Rows, scope);

        return result.Rows.FirstOrDefault();
    }

    /// <summary>
    /// Availability data of all documents matching the criteria in the scope, by document id.
    /// The stages run once, the target is read in pages of <see cref="StatsPageSize"/>.
    /// </summary>
    /// <exception cref="NotSupportedException">The document has no "Id" and "Data" properties.</exception>
    public async Task<IReadOnlyDictionary<object, DataIndex>> Stats(CriteriaContainer criteria, SearchScope scope, string term = null)
    {
        if (_id == null || _data == null)
            throw new NotSupportedException($"{typeof(T).Name} has no Id and Data properties.");

        var stats = new Dictionary<object, DataIndex>();
        var targetCriteria = await CreateTargetCriteria(criteria, scope);

        if (targetCriteria == null)
            return stats;

        for (var from = 0; ; from += StatsPageSize)
        {
            var page = (await Query(targetCriteria, from, StatsPageSize, term)).Rows.ToList();

            foreach (var document in page)
                stats[_id(document)] = _data(document);

            if (page.Count < StatsPageSize)
                return stats;
        }
    }

    /// <summary>
    /// Stages of all documents with criteria except the target, in execution order (see <see cref="SearchTrees.GetRank"/>):
    /// the main tree from its root to the finest entity; at the main-tree entity a secondary tree is attached to, the entities
    /// of that tree from its root; then the other documents of the level (e.g. donors, projects, specimens, genes, proteins,
    /// gene expressions, ...). Ties are ordered by document name; a document's include stage runs before its exclude stage,
    /// which is then narrowed by the keys just found.
    /// </summary>
    /// <exception cref="ArgumentException">Criteria of a document can't be applied to the target.</exception>
    public IReadOnlyList<SearchStage> Plan(CriteriaContainer criteria)
    {
        return criteria.DocumentTypes
            .Where(documentType => documentType != typeof(T))
            .SelectMany(documentType =>
            {
                var documentCriteria = criteria.Get(documentType);
                var stages = new List<SearchStage>();

                if (documentCriteria.Include != null)
                    stages.Add(CreateStage(documentType, false));

                if (documentCriteria.Exclude != null)
                    stages.Add(CreateStage(documentType, true));

                return stages;
            })
            .OrderBy(stage => stage.Rank)
            .ThenBy(stage => stage.Stage.Document.Name, StringComparer.Ordinal)
            .ThenBy(stage => stage.IsExclusion)
            .ToList();
    }


    /// <summary>
    /// Search stage of the document: its join with the target, the keys it collects and its rank.
    /// </summary>
    private static SearchStage CreateStage(Type documentType, bool exclusion)
    {
        var targetEntities = TargetEntities;
        var stage = DocumentStages.Get(documentType);
        var join = GetJoin(stage, targetEntities, TargetRoot);
        var identity = stage.Keys.FirstOrDefault(key => key.IsIdentity);
        var collects = identity == null || identity == join ? new[] { join } : new[] { join, identity };
        var bridge = targetEntities.Contains(join.Entity.Document.Type) ? null : GetBridge(join.Entity, targetEntities);

        return new SearchStage(stage, exclusion, join, collects, bridge, SearchTrees.GetRank(stage));
    }

    /// <summary>
    /// Criteria of the target combining its own criteria, the access restriction and the keys found and excluded by the stages;
    /// null if no target document can match.
    /// </summary>
    private async Task<CriteriaGroup> CreateTargetCriteria(CriteriaContainer criteria, SearchScope scope)
    {
        var plan = Plan(criteria);
        var restriction = await GetRestriction(scope, plan.Count > 0);

        if (restriction?.IsNothing == true)
            return null;

        var found = new Dictionary<Type, HashSet<string>>();
        var excluded = new Dictionary<Type, HashSet<string>>();
        var bridged = new HashSet<Type>();
        CriteriaGroup targetRestriction = null;

        // The access restriction runs first and narrows all stages; a restriction on the target itself goes into its query.
        if (restriction != null && restriction.DocumentType == typeof(T))
        {
            targetRestriction = restriction.Criteria;
        }
        else if (restriction != null)
        {
            var stage = CreateStage(restriction.DocumentType, false);
            var keys = await stage.Stage.FindKeys(restriction.Criteria, stage.Collects, found, excluded, _serviceProvider);

            if (stage.Bridge != null)
                bridged.Add(stage.Join.Entity.Document.Type);

            foreach (var entry in keys)
            {
                if (!Include(found, entry.Key, entry.Value))
                    return null;
            }
        }

        // Stages narrow each other through the keys found and excluded so far; found keys of the same entity are intersected.
        foreach (var stage in plan)
        {
            var keys = await stage.Stage.FindKeys(criteria, stage.IsExclusion, stage.Collects, found, excluded, _serviceProvider);

            if (keys == null)
                continue;

            if (stage.Bridge != null)
                bridged.Add(stage.Join.Entity.Document.Type);

            foreach (var entry in keys)
            {
                if (stage.IsExclusion ? !Exclude(excluded, entry.Key, entry.Value) : !Include(found, entry.Key, entry.Value))
                    return null;
            }
        }

        // Join keys of stages sharing no entity with the target are mapped to the nearest main-tree entity it refers to.
        // Identity keys collected besides a join key only narrow later stages and are not mapped.
        var targetEntities = TargetEntities;

        foreach (var entry in found.Where(entry => bridged.Contains(entry.Key)).ToList())
        {
            var bridge = GetBridge(SearchEntities.All[entry.Key], targetEntities);

            if (!Include(found, bridge.Document.Type, await MapKeys(entry.Key, entry.Value, bridge.Document.Type)))
                return null;
        }

        foreach (var entry in excluded.Where(entry => bridged.Contains(entry.Key)).ToList())
        {
            var bridge = GetBridge(SearchEntities.All[entry.Key], targetEntities);

            Exclude(excluded, bridge.Document.Type, await MapKeys(entry.Key, entry.Value, bridge.Document.Type));
        }

        return CreateTargetCriteria(criteria.Get<T>(), targetRestriction, found, excluded);
    }

    /// <exception cref="ArgumentException">A free text is given, but the target has no searchable text fields.</exception>
    private async Task<SearchResult<T>> Query(CriteriaGroup targetCriteria, int from, int size, string term,
        IEnumerable<Expression<Func<T, object>>> exclusions = null)
    {
        var searchQuery = new SearchQuery<T>()
            .AddPagination(from, size)
            .AddFilter(DocumentCriteriaConverter.Convert<T>(targetCriteria));

        if (!string.IsNullOrWhiteSpace(term))
        {
            if (!SearchResults<T>.TextFields.Any())
                throw new ArgumentException($"'{SearchDocuments.Get(typeof(T)).Name}' has no searchable text fields for a free-text search.");

            searchQuery.AddFilter(new FreeTextFilter<T>(term));
        }

        foreach (var (property, descending) in SearchResults<T>.Orders)
            searchQuery.AddOrdering(property, !descending);

        foreach (var exclusion in exclusions ?? [])
            searchQuery.AddExclusion(exclusion);

        return await _serviceProvider.GetRequiredService<IIndexService<T>>().Search(searchQuery);
    }

    /// <summary>
    /// Access restriction of the search, null if not restricted.
    /// </summary>
    /// <exception cref="InvalidOperationException">The scope is restricted, but no access service is registered.</exception>
    private async Task<AccessRestriction> GetRestriction(SearchScope scope, bool hasDocumentCriteria)
    {
        ArgumentNullException.ThrowIfNull(scope);

        if (scope.IsAll)
            return null;

        return await GetAccessService().GetRestriction(scope, typeof(T), hasDocumentCriteria);
    }

    private async Task Annotate(IEnumerable<T> documents, SearchScope scope)
    {
        var accessService = _serviceProvider.GetService<IAccessService>();

        if (accessService != null)
            await accessService.Annotate(documents, scope);
    }

    private IAccessService GetAccessService()
    {
        // Searches of users must never run unrestricted because the access service is missing.
        return _serviceProvider.GetService<IAccessService>()
            ?? throw new InvalidOperationException($"No {nameof(IAccessService)} is registered: searches of users can't be restricted.");
    }

    private static HashSet<Type> TargetEntities => DocumentKeys<T>.All.Keys.ToHashSet();

    private static SearchDocument TargetRoot => SearchEntities.All.TryGetValue(typeof(T), out var entity) ? entity.Root : null;

    /// <summary>
    /// Criteria: the field has the value, null if the value can't be read as the field type (e.g. "abc" for an int id).
    /// </summary>
    private static PropertyCriteria CreateValueCriteria<TValue>(SearchField field, string value)
    {
        try
        {
            var typed = (TValue)TypeDescriptor.GetConverter(typeof(TValue)).ConvertFromInvariantString(value);

            return new ValueCriteria<TValue> { Property = field, Values = [typed] };
        }
        catch (Exception exception) when (exception is FormatException or NotSupportedException or ArgumentException || exception.InnerException is FormatException or OverflowException)
        {
            return null;
        }
    }

    /// <summary>
    /// Entity joining the document with the target: the document itself if the target refers to it,
    /// otherwise the finest entity both refer to in the target's own hierarchy (e.g. genes for gene expressions in a gene search),
    /// otherwise the finest entity of the main tree both refer to. If they share no entity, the finest main-tree entity of the
    /// document, which is then bridged to the target along the main tree (see <see cref="GetBridge"/>).
    /// </summary>
    private static DocumentKey GetJoin(IDocumentStage stage, HashSet<Type> targetEntities, SearchDocument targetRoot)
    {
        var candidates = stage.Keys
            .Where(key => targetEntities.Contains(key.Entity.Document.Type))
            .ToList();

        var sameHierarchy = candidates.Where(key => key.Entity.Root == targetRoot).ToList();

        // Without a shared hierarchy, main-tree entities are preferred (e.g. protein expressions join an SM search by specimen, not by protein).
        return candidates.FirstOrDefault(key => key.IsIdentity)
            ?? (sameHierarchy.Count > 0 ? sameHierarchy : candidates)
                .OrderByDescending(key => key.Entity.Root == SearchTrees.Main)
                .ThenByDescending(key => key.Entity.Level)
                .FirstOrDefault()
            ?? stage.Keys
                .Where(key => key.Entity.Root == SearchTrees.Main)
                .OrderByDescending(key => key.Entity.Level)
                .FirstOrDefault()
            ?? throw new ArgumentException($"Criteria of '{stage.Document.Name}' can't be applied to '{SearchDocuments.Get(typeof(T)).Name}': they share no searchable entity.");
    }

    /// <summary>
    /// Main-tree entity of the target nearest to the given main-tree entity (fewest parent links between them, then the finer one).
    /// </summary>
    private static SearchEntity GetBridge(SearchEntity entity, HashSet<Type> targetEntities)
    {
        return targetEntities
            .Select(type => SearchEntities.All[type])
            .Where(target => target.Root == SearchTrees.Main)
            .OrderBy(target => GetPath(entity.Document.Type, target.Document.Type).Count)
            .ThenByDescending(target => target.Level)
            .FirstOrDefault()
            ?? throw new ArgumentException($"Criteria on '{entity.Document.Name}' can't be applied to '{SearchDocuments.Get(typeof(T)).Name}': it refers to no entity of the main tree.");
    }

    /// <summary>
    /// Maps keys of one main-tree entity to keys of another, link by link through the child entity's documents
    /// (up: specimen keys to donor keys via specimens.donor; down: donor keys to specimen keys via specimens.donor).
    /// </summary>
    private async Task<HashSet<string>> MapKeys(Type fromEntity, HashSet<string> keys, Type toEntity)
    {
        var path = GetPath(fromEntity, toEntity);

        for (var index = 0; index < path.Count - 1 && keys.Count > 0; index++)
        {
            var (current, next) = (SearchEntities.All[path[index]], SearchEntities.All[path[index + 1]]);
            var up = current.Parent == next.Document.Type;
            var child = DocumentStages.Get(up ? current.Document.Type : next.Document.Type);
            var fromKey = child.Keys.Single(key => key.Entity == current);
            var toKey = child.Keys.Single(key => key.Entity == next);

            keys = await child.MapKeys(fromKey, keys, toKey, _serviceProvider);
        }

        return keys;
    }

    /// <summary>
    /// Entities from one entity to another through their parent links (both included), up to the closest common ancestor and down.
    /// </summary>
    private static List<Type> GetPath(Type fromEntity, Type toEntity)
    {
        static List<Type> Ancestors(Type entity)
        {
            var ancestors = new List<Type>();

            for (var current = entity; current != null; current = SearchEntities.All[current].Parent)
                ancestors.Add(current);

            return ancestors;
        }

        var up = Ancestors(fromEntity);
        var down = Ancestors(toEntity);
        var common = up.First(down.Contains);

        return up.TakeWhile(type => type != common).Append(common).Concat(down.TakeWhile(type => type != common).Reverse()).ToList();
    }

    private static bool Include(Dictionary<Type, HashSet<string>> found, Type entity, HashSet<string> keys)
    {
        // Documents were filtered, but nothing was found: no target document can match.
        if (keys.Count == 0)
            return false;

        if (found.TryGetValue(entity, out var previous))
            keys.IntersectWith(previous);

        found[entity] = keys;

        // Keys found before and now have nothing in common: no target document can match.
        return keys.Count > 0;
    }

    private static bool Exclude(Dictionary<Type, HashSet<string>> excluded, Type entity, HashSet<string> keys)
    {
        if (excluded.TryGetValue(entity, out var previous))
            previous.UnionWith(keys);
        else
            excluded[entity] = keys;

        return true;
    }

    /// <summary>
    /// Combines the target's own criteria with the keys found and excluded for its entities (AND). The input criteria are not modified.
    /// </summary>
    /// <summary>
    /// Getter of a property of the document, null if the document has no such property.
    /// </summary>
    private static Func<T, TValue> CreateGetter<TValue>(string name)
    {
        var property = typeof(T).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

        if (property == null || !typeof(TValue).IsAssignableFrom(property.PropertyType) && typeof(TValue) != typeof(object))
            return null;

        var document = Expression.Parameter(typeof(T), "document");

        return Expression.Lambda<Func<T, TValue>>(Expression.Convert(Expression.Property(document, property), typeof(TValue)), document).Compile();
    }

    private static CriteriaGroup CreateTargetCriteria(DocumentCriteria<T> ownCriteria, CriteriaGroup restriction, Dictionary<Type, HashSet<string>> found, Dictionary<Type, HashSet<string>> excluded)
    {
        var nodes = new List<CriteriaNode>();

        if (restriction != null)
            nodes.Add(restriction);

        if (ownCriteria?.Include != null)
            nodes.Add(ownCriteria.Include);

        // Target documents matching their own exclude criteria are excluded.
        if (ownCriteria?.Exclude != null)
            nodes.Add(new CriteriaGroup { Negate = true, Nodes = [ownCriteria.Exclude] });

        // Keys of entities the target does not refer to (e.g. genes in an image search) only narrowed other stages.
        foreach (var entry in found)
        {
            if (DocumentKeys<T>.All.TryGetValue(entry.Key, out var key))
                nodes.Add(key.CreateCriteria(entry.Value));
        }

        foreach (var entry in excluded)
        {
            if (DocumentKeys<T>.All.TryGetValue(entry.Key, out var key) && entry.Value.Count > 0)
            {
                var exclusion = key.CreateCriteria(entry.Value);
                exclusion.Negate = true;
                nodes.Add(exclusion);
            }
        }

        return new CriteriaGroup { Operator = LogicalOperator.And, Nodes = nodes };
    }
}
