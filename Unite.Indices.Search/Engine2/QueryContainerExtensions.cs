using System.Reflection;
using Unite.Indices.Search.Engine2.InputFilters;

namespace Unite.Indices.Search.Engine2;

/// <summary>
/// Access to document queries of a <see cref="QueryContainer"/> by document type.
/// </summary>
public static class QueryContainerExtensions
{
    private static readonly Dictionary<Type, PropertyInfo> _documentQueryProperties = typeof(QueryContainer)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(property => property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(DocumentQuery<>))
        .ToDictionary(property => property.PropertyType.GetGenericArguments()[0]);


    public static IDocumentQuery Get(this QueryContainer query, Type documentType)
    {
        return _documentQueryProperties.TryGetValue(documentType, out var property)
            ? (IDocumentQuery)property.GetValue(query)
            : null;
    }

    public static DocumentQuery<TDocument> Get<TDocument>(this QueryContainer query)
    {
        return (DocumentQuery<TDocument>)query.Get(typeof(TDocument));
    }

    public static void Set<TDocument>(this QueryContainer query, DocumentQuery<TDocument> documentQuery)
    {
        if (!_documentQueryProperties.TryGetValue(typeof(TDocument), out var property))
            throw new NotSupportedException($"Document type '{typeof(TDocument).Name}' is not supported.");

        property.SetValue(query, documentQuery);
    }

    /// <summary>
    /// Creates a shallow copy: document queries are shared, replace them with <see cref="Set{TDocument}"/> instead of modifying.
    /// </summary>
    public static QueryContainer Copy(this QueryContainer query)
    {
        var copy = new QueryContainer();

        foreach (var property in _documentQueryProperties.Values)
            property.SetValue(copy, property.GetValue(query));

        return copy;
    }
}
