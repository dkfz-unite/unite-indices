using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Converters;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.References;

/// <summary>
/// Search stage of a referenced document: finds entity keys of the documents matching its criteria.
/// </summary>
public interface IDocumentStage
{
    SearchDocument Document { get; }

    /// <summary>
    /// Entity keys of the document.
    /// </summary>
    IEnumerable<DocumentKey> Keys { get; }

    /// <summary>
    /// Finds the given entity keys in documents matching the document criteria and the keys found so far, with one request.
    /// Criteria are searched as written; for exclusion criteria (<see cref="DocumentCriteria.IsExclusion"/>) the caller excludes the found keys.
    /// Returns null if the document has no criteria (nothing to constrain).
    /// </summary>
    Task<IReadOnlyDictionary<Type, HashSet<string>>> FindKeys(CriteriaContainer criteria, IEnumerable<DocumentKey> keys,
        IReadOnlyDictionary<Type, HashSet<string>> found, IReadOnlyDictionary<Type, HashSet<string>> excluded, IServiceProvider serviceProvider);

    /// <summary>
    /// Maps keys of one entity to keys of another entity through documents of this type (e.g. specimen keys to their donor keys).
    /// </summary>
    Task<HashSet<string>> MapKeys(DocumentKey from, IEnumerable<string> keys, DocumentKey to, IServiceProvider serviceProvider);
}

public static class DocumentStages
{
    private static readonly ConcurrentDictionary<Type, IDocumentStage> _stages = new();

    public static IDocumentStage Get(Type documentType)
    {
        return _stages.GetOrAdd(documentType, type => (IDocumentStage)Activator.CreateInstance(typeof(DocumentStage<>).MakeGenericType(type)));
    }
}

internal sealed class DocumentStage<TDocument> : IDocumentStage where TDocument : class
{
    public SearchDocument Document { get; } = SearchDocuments.Get(typeof(TDocument));

    public IEnumerable<DocumentKey> Keys => DocumentKeys<TDocument>.All.Values;


    public async Task<IReadOnlyDictionary<Type, HashSet<string>>> FindKeys(CriteriaContainer criteria, IEnumerable<DocumentKey> keys,
        IReadOnlyDictionary<Type, HashSet<string>> found, IReadOnlyDictionary<Type, HashSet<string>> excluded, IServiceProvider serviceProvider)
    {
        var ownCriteria = criteria.Get<TDocument>();

        if (ownCriteria == null || DocumentCriteriaConverter.Convert(ownCriteria).IsEmpty)
            return null;

        var nodes = new List<CriteriaNode> { ownCriteria };

        // Keys found and excluded in earlier stages narrow this stage (e.g. gene expressions only of the genes and specimens found so far).
        foreach (var entry in found)
        {
            if (DocumentKeys<TDocument>.All.TryGetValue(entry.Key, out var key))
                nodes.Add(key.CreateCriteria(entry.Value));
        }

        foreach (var entry in excluded)
        {
            if (DocumentKeys<TDocument>.All.TryGetValue(entry.Key, out var key))
            {
                var exclusion = key.CreateCriteria(entry.Value);
                exclusion.Negate = true;
                nodes.Add(exclusion);
            }
        }

        return await Aggregate(nodes, keys.Cast<DocumentKey<TDocument>>(), serviceProvider);
    }

    public async Task<HashSet<string>> MapKeys(DocumentKey from, IEnumerable<string> keys, DocumentKey to, IServiceProvider serviceProvider)
    {
        var nodes = new List<CriteriaNode> { ((DocumentKey<TDocument>)from).CreateCriteria(keys) };
        var result = await Aggregate(nodes, [(DocumentKey<TDocument>)to], serviceProvider);

        return result[to.Entity.Document.Type];
    }


    /// <summary>
    /// Aggregates the keys of all documents matching all nodes, with one request.
    /// </summary>
    private static async Task<IReadOnlyDictionary<Type, HashSet<string>>> Aggregate(List<CriteriaNode> nodes, IEnumerable<DocumentKey<TDocument>> keys,
        IServiceProvider serviceProvider)
    {
        var criteria = new DocumentCriteria<TDocument> { Operator = LogicalOperator.And, Nodes = nodes };

        var query = new SearchQuery<TDocument>()
            .AddPagination(0, 0)
            .AddFilter(DocumentCriteriaConverter.Convert(criteria));

        // Aggregations are named after the entity ("donors_keys"); "keys" alone is reserved by NEST.
        var aggregations = keys.ToDictionary(key => key.Entity.Document.Type, key => (Key: key, Name: $"{key.Entity.Document.Name}_keys"));

        foreach (var aggregation in aggregations.Values)
            aggregation.Key.AddAggregation(query, aggregation.Name);

        var result = await serviceProvider.GetRequiredService<IIndexService<TDocument>>().Search(query);

        return aggregations.ToDictionary(
            entry => entry.Key,
            entry => result.Aggregations.TryGetValue(entry.Value.Name, out var buckets) && buckets != null ? buckets.Keys.ToHashSet() : []);
    }
}
