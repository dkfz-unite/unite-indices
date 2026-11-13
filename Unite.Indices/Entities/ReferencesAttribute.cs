namespace Unite.Indices.Entities;

/// <summary>
/// Marks a navigation property (object or collection) as a reference to a searchable entity.
/// The referenced entity is identified by the key property of the navigation object (<see cref="EntityKeyAttribute"/>).
/// </summary>
/// <param name="entity">Searchable entity document type (<see cref="SearchableDocumentAttribute"/> with an <see cref="EntityKeyAttribute"/> property).</param>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class ReferencesAttribute(Type entity) : Attribute
{
    public Type Entity { get; } = entity;
}
