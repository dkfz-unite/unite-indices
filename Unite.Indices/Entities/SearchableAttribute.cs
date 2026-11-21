namespace Unite.Indices.Entities;

/// <summary>
/// Marks an index property as searchable.
/// A nested property is searchable only if every property on the path from the document to it is marked:
/// marking a navigation property (object or collection) allows searching its marked properties.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class SearchableAttribute : Attribute
{
    /// <summary>
    /// Name of the path segment in search criteria. Defaults to the camel-cased property name.
    /// </summary>
    public string Name { get; set; }
}
