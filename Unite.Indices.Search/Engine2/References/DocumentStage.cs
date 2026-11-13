using System.Collections.Concurrent;
using Unite.Indices.Search.Engine2.Converters;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.References;

/// <summary>
/// Search stage of a referenced document: finds keys of the entity joining it with the search target.
/// </summary>
public interface IDocumentStage
{
    SearchDocument Document { get; }

    /// <summary>
    /// Entity keys of the document.
    /// </summary>
    IEnumerable<DocumentKey> Keys { get; }

    /// <summary>
    /// Finds keys of the join entity in documents matching the document criteria and the keys found so far.
    /// Returns null if the document has no criteria (nothing to constrain).
    /// </summary>
    Task<HashSet<string>> FindKeys(CriteriaContainer criteria, DocumentKey join, IReadOnlyDictionary<Type, HashSet<string>> found, IServiceProvider serviceProvider);
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


    public async Task<HashSet<string>> FindKeys(CriteriaContainer criteria, DocumentKey join, IReadOnlyDictionary<Type, HashSet<string>> found, IServiceProvider serviceProvider)
    {
        var ownCriteria = criteria.Get<TDocument>();

        if (ownCriteria == null || DocumentCriteriaConverter.Convert(ownCriteria).IsEmpty)
            return null;

        var nodes = new List<CriteriaNode> { ownCriteria };

        // Keys found in earlier stages narrow this stage (e.g. genes only in the specimens found so far).
        foreach (var entry in found)
        {
            if (DocumentKeys<TDocument>.All.TryGetValue(entry.Key, out var key))
                nodes.Add(key.CreateCriteria(entry.Value));
        }

        var stageCriteria = new DocumentCriteria<TDocument> { Operator = LogicalOperator.And, Nodes = nodes };

        return await ((DocumentKey<TDocument>)join).FindKeys(stageCriteria, serviceProvider);
    }
}
