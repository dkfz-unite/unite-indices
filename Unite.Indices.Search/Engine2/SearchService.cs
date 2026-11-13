using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Converters;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.References;

namespace Unite.Indices.Search.Engine2;

/// <summary>
/// Searches documents of type <typeparamref name="T"/> with criteria on any searchable documents.
/// Criteria of other documents are applied through the entities (donors, specimens) they share with the target,
/// derived from <see cref="Unite.Indices.Entities.SearchableDocumentAttribute"/> and <see cref="Unite.Indices.Entities.ReferencesAttribute"/>.
/// </summary>
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
        var targetKeys = DocumentKeys<T>.All;
        var found = new Dictionary<Type, HashSet<string>>();

        // Stages run from coarse to fine entities and narrow each other through the keys found so far.
        foreach (var (stage, join) in GetStages(criteria, targetKeys.Keys))
        {
            var keys = await stage.FindKeys(criteria, join, found, _serviceProvider);

            if (keys == null)
                continue;

            if (found.TryGetValue(join.Entity.Document.Type, out var previous))
                keys.IntersectWith(previous);

            // Referenced documents were filtered, but nothing was found: no target document can match.
            if (keys.Count == 0)
                return new SearchResult<T>();

            found[join.Entity.Document.Type] = keys;
        }

        var targetCriteria = CreateTargetCriteria(criteria.Get<T>(), found);

        var searchQuery = new SearchQuery<T>()
            .AddPagination(from, size)
            .AddFilter(DocumentCriteriaConverter.Convert(targetCriteria));

        var indexService = _serviceProvider.GetRequiredService<IIndexService<T>>();

        return await indexService.Search(searchQuery);
    }


    /// <summary>
    /// Stages of all documents with criteria except the target, with the entity joining each with the target, in execution order.
    /// </summary>
    private static List<(IDocumentStage Stage, DocumentKey Join)> GetStages(CriteriaContainer criteria, IEnumerable<Type> targetEntities)
    {
        var entities = targetEntities.ToHashSet();

        return criteria.DocumentTypes
            .Where(documentType => documentType != typeof(T))
            .Select(DocumentStages.Get)
            .Select(stage => (Stage: stage, Join: GetJoin(stage, entities)))
            .OrderBy(entry => entry.Join.Entity.Level)
            .ThenBy(entry => entry.Join.IsIdentity ? 0 : 1)
            .ThenBy(entry => entry.Stage.Document.Name, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// Entity joining the document with the target: the document itself if the target refers to it,
    /// otherwise the finest entity both refer to.
    /// </summary>
    private static DocumentKey GetJoin(IDocumentStage stage, HashSet<Type> targetEntities)
    {
        var candidates = stage.Keys
            .Where(key => targetEntities.Contains(key.Entity.Document.Type))
            .ToList();

        return candidates.FirstOrDefault(key => key.IsIdentity)
            ?? candidates.OrderByDescending(key => key.Entity.Level).FirstOrDefault()
            ?? throw new ArgumentException($"Criteria of '{stage.Document.Name}' can't be applied to '{SearchDocumentName}': they share no searchable entity.");
    }

    /// <summary>
    /// Combines the target's own criteria with the keys found in other documents (AND). The input criteria are not modified.
    /// </summary>
    private static DocumentCriteria<T> CreateTargetCriteria(DocumentCriteria<T> ownCriteria, Dictionary<Type, HashSet<string>> found)
    {
        var nodes = new List<CriteriaNode>();

        if (ownCriteria != null)
            nodes.Add(ownCriteria);

        foreach (var entry in found)
            nodes.Add(DocumentKeys<T>.All[entry.Key].CreateCriteria(entry.Value));

        return new DocumentCriteria<T> { Operator = LogicalOperator.And, Nodes = nodes };
    }

    private static string SearchDocumentName => Fields.SearchDocuments.Get(typeof(T)).Name;
}
