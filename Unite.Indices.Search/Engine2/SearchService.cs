using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Converters;
using Unite.Indices.Search.Engine2.Criteria;
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
/// </remarks>
public class SearchService<T> where T : class
{
    private readonly IServiceProvider _serviceProvider;


    public SearchService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    /// <exception cref="ArgumentException">Criteria of a document can't be applied to the target or are invalid.</exception>
    public async Task<SearchResult<T>> Search(CriteriaContainer criteria, int from = 0, int size = 20)
    {
        var found = new Dictionary<Type, HashSet<string>>();
        var excluded = new Dictionary<Type, HashSet<string>>();

        // Stages narrow each other through the keys found and excluded so far; found keys of the same entity are intersected.
        foreach (var stage in Plan(criteria))
        {
            var keys = await stage.Stage.FindKeys(criteria, stage.IsExclusion, stage.Collects, found, excluded, _serviceProvider);

            if (keys == null)
                continue;

            foreach (var entry in keys)
            {
                if (stage.IsExclusion ? !Exclude(excluded, entry.Key, entry.Value) : !Include(found, entry.Key, entry.Value))
                    return new SearchResult<T>();
            }
        }

        // Keys of main-tree entities the target does not refer to are mapped to the nearest one it refers to.
        var targetEntities = DocumentKeys<T>.All.Keys.ToHashSet();

        foreach (var entry in found.Where(entry => IsBridged(entry.Key, targetEntities)).ToList())
        {
            var bridge = GetBridge(SearchEntities.All[entry.Key], targetEntities);

            if (!Include(found, bridge.Document.Type, await MapKeys(entry.Key, entry.Value, bridge.Document.Type)))
                return new SearchResult<T>();
        }

        foreach (var entry in excluded.Where(entry => IsBridged(entry.Key, targetEntities)).ToList())
        {
            var bridge = GetBridge(SearchEntities.All[entry.Key], targetEntities);

            Exclude(excluded, bridge.Document.Type, await MapKeys(entry.Key, entry.Value, bridge.Document.Type));
        }

        var targetCriteria = CreateTargetCriteria(criteria.Get<T>(), found, excluded);

        var searchQuery = new SearchQuery<T>()
            .AddPagination(from, size)
            .AddFilter(DocumentCriteriaConverter.Convert<T>(targetCriteria));

        var indexService = _serviceProvider.GetRequiredService<IIndexService<T>>();

        return await indexService.Search(searchQuery);
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
        var targetEntities = DocumentKeys<T>.All.Keys.ToHashSet();
        var targetRoot = SearchEntities.All.TryGetValue(typeof(T), out var targetEntity) ? targetEntity.Root : null;

        return criteria.DocumentTypes
            .Where(documentType => documentType != typeof(T))
            .SelectMany(documentType =>
            {
                var documentCriteria = criteria.Get(documentType);
                var stage = DocumentStages.Get(documentType);
                var join = GetJoin(stage, targetEntities, targetRoot);
                var identity = stage.Keys.FirstOrDefault(key => key.IsIdentity);
                var collects = identity == null || identity == join ? new[] { join } : new[] { join, identity };
                var bridge = targetEntities.Contains(join.Entity.Document.Type) ? null : GetBridge(join.Entity, targetEntities);
                var rank = SearchTrees.GetRank(stage);

                var stages = new List<SearchStage>();

                if (documentCriteria.Include != null)
                    stages.Add(new SearchStage(stage, false, join, collects, bridge, rank));

                if (documentCriteria.Exclude != null)
                    stages.Add(new SearchStage(stage, true, join, collects, bridge, rank));

                return stages;
            })
            .OrderBy(stage => stage.Rank)
            .ThenBy(stage => stage.Stage.Document.Name, StringComparer.Ordinal)
            .ThenBy(stage => stage.IsExclusion)
            .ToList();
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

    private static bool IsBridged(Type entityType, HashSet<Type> targetEntities)
    {
        return !targetEntities.Contains(entityType) && SearchEntities.All[entityType].Root == SearchTrees.Main;
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
    private static CriteriaGroup CreateTargetCriteria(DocumentCriteria<T> ownCriteria, Dictionary<Type, HashSet<string>> found, Dictionary<Type, HashSet<string>> excluded)
    {
        var nodes = new List<CriteriaNode>();

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
