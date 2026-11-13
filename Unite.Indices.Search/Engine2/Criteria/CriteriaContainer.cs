using System.Text.Json.Serialization;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.Criteria;

/// <summary>
/// Criteria per searchable document type.
/// In JSON an object with one property per document, named as the document (e.g. "images", "specimens").
/// </summary>
[JsonConverter(typeof(CriteriaContainerJsonConverter))]
public class CriteriaContainer
{
    private readonly Dictionary<Type, CriteriaGroup> _criteria = [];

    /// <summary>
    /// Document types with criteria.
    /// </summary>
    public IEnumerable<Type> DocumentTypes => _criteria.Keys;


    public DocumentCriteria<TDocument> Get<TDocument>() where TDocument : class
    {
        return (DocumentCriteria<TDocument>)Get(typeof(TDocument));
    }

    public CriteriaGroup Get(Type documentType)
    {
        return _criteria.TryGetValue(documentType, out var criteria) ? criteria : null;
    }

    /// <summary>
    /// Sets criteria for the document type, null removes them.
    /// </summary>
    /// <exception cref="NotSupportedException">The type is not a searchable document.</exception>
    public void Set<TDocument>(DocumentCriteria<TDocument> criteria) where TDocument : class
    {
        Set(typeof(TDocument), criteria);
    }

    internal void Set(Type documentType, CriteriaGroup criteria)
    {
        SearchDocuments.Get(documentType);

        if (criteria == null)
            _criteria.Remove(documentType);
        else
            _criteria[documentType] = criteria;
    }

    /// <summary>
    /// Creates a shallow copy: document criteria are shared, replace them with <see cref="Set{TDocument}"/> instead of modifying.
    /// </summary>
    public CriteriaContainer Copy()
    {
        var copy = new CriteriaContainer();

        foreach (var entry in _criteria)
            copy._criteria[entry.Key] = entry.Value;

        return copy;
    }
}
