namespace Unite.Indices.Entities;

/// <summary>
/// Marks an index document class as searchable under the given name (e.g. "images").
/// Not inherited: only the marked class itself is a searchable document.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SearchableDocumentAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
