namespace Unite.Indices.Entities;

/// <summary>
/// Marks an index document class as searchable under the given name (e.g. "images").
/// A document with a property marked with <see cref="EntityKeyAttribute"/> is also an entity other documents can refer to (e.g. donors, specimens).
/// Not inherited: only the marked class itself is a searchable document.
/// </summary>
/// <remarks>
/// Entities form a hierarchy derived from their references: an entity referring to another entity
/// with a single object (not a collection) is its child, e.g. specimens are children of donors (SpecimenIndex.Donor).
/// The entity without a parent is the root. Search stages run from the root to finer entities,
/// documents referring to several entities are joined by the finest one.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SearchableDocumentAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
