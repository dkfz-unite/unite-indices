using System.Reflection;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine2.Criteria;

namespace Unite.Indices.Search.Engine2.Fields;

/// <summary>
/// Searchable document types, found by <see cref="SearchableDocumentAttribute"/> in the index models assembly.
/// </summary>
public static class SearchDocuments
{
    private static readonly Lazy<IReadOnlyList<SearchDocument>> _documents = new(Build);

    public static IReadOnlyList<SearchDocument> All => _documents.Value;


    public static SearchDocument Find(string name)
    {
        return All.FirstOrDefault(document => string.Equals(document.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    public static SearchDocument Find(Type type)
    {
        return All.FirstOrDefault(document => document.Type == type);
    }

    /// <exception cref="NotSupportedException">The type is not a searchable document.</exception>
    public static SearchDocument Get(Type type)
    {
        return Find(type) ?? throw new NotSupportedException($"{type.FullName} is not a searchable document.");
    }


    private static IReadOnlyList<SearchDocument> Build()
    {
        var documents = typeof(SearchableDocumentAttribute).Assembly.GetTypes()
            .Select(type => (Type: type, Attribute: type.GetCustomAttribute<SearchableDocumentAttribute>(false)))
            .Where(entry => entry.Attribute != null)
            .Select(entry => new SearchDocument(entry.Attribute.Name, entry.Type))
            .ToList();

        var duplicate = documents
            .GroupBy(document => document.Name, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicate != null)
            throw new InvalidOperationException($"Searchable document name '{duplicate.Key}' is declared on {string.Join(", ", duplicate.Select(document => document.Type.FullName))}.");

        return documents;
    }
}

/// <summary>
/// Searchable document type with its name in criteria and the type of its criteria.
/// </summary>
public sealed class SearchDocument(string name, Type type)
{
    public string Name { get; } = name;
    public Type Type { get; } = type;
    public Type CriteriaType { get; } = typeof(DocumentCriteria<>).MakeGenericType(type);
}
