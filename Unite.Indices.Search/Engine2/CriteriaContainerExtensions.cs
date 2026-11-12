using System.Reflection;
using Unite.Indices.Search.Engine2.Criteria;

namespace Unite.Indices.Search.Engine2;

/// <summary>
/// Access to document queries of a <see cref="CriteriaContainer"/> by document type.
/// </summary>
public static class CriteriaContainerExtensions
{
    private static readonly Dictionary<Type, PropertyInfo> _documentCriteriaProperties = typeof(CriteriaContainer)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(property => property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(DocumentCriteria<>))
        .ToDictionary(property => property.PropertyType.GetGenericArguments()[0]);


    public static CriteriaNode Get(this CriteriaContainer criteria, Type documentType)
    {
        return _documentCriteriaProperties.TryGetValue(documentType, out var property)
            ? (CriteriaNode)property.GetValue(criteria)
            : null;
    }

    public static DocumentCriteria<TDocument> Get<TDocument>(this CriteriaContainer criteria)
    {
        return (DocumentCriteria<TDocument>)criteria.Get(typeof(TDocument));
    }

    public static void Set<TDocument>(this CriteriaContainer criteria, DocumentCriteria<TDocument> documentCriteria)
    {
        if (!_documentCriteriaProperties.TryGetValue(typeof(TDocument), out var property))
            throw new NotSupportedException($"Document type '{typeof(TDocument).Name}' is not supported.");

        property.SetValue(criteria, documentCriteria);
    }

    /// <summary>
    /// Creates a shallow copy: document queries are shared, replace them with <see cref="Set{TDocument}"/> instead of modifying.
    /// </summary>
    public static CriteriaContainer Copy(this CriteriaContainer criteria)
    {
        var copy = new CriteriaContainer();

        foreach (var property in _documentCriteriaProperties.Values)
            property.SetValue(copy, property.GetValue(criteria));

        return copy;
    }
}
