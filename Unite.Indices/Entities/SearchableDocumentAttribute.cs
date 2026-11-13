namespace Unite.Indices.Entities;

/// <summary>
/// Marks an index document class as searchable under the given name (e.g. "images").
/// With <see cref="Key"/> and <see cref="Level"/> the document is also an entity other documents can refer to (e.g. donors, specimens).
/// Not inherited: only the marked class itself is a searchable document.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SearchableDocumentAttribute(string name) : Attribute
{
    public string Name { get; } = name;

    /// <summary>
    /// Name of the key property, if the document is an entity (e.g. nameof(DonorIndex.Id)).
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// Granularity of the entity, required with <see cref="Key"/>: higher is finer (e.g. donors 1, specimens 2). Must be unique.
    /// Search stages run from coarse to fine entities, documents referring to several entities are joined by the finest one.
    /// </summary>
    public int Level { get; set; }
}
