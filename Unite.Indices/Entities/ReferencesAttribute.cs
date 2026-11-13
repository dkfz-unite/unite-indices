namespace Unite.Indices.Entities;

/// <summary>
/// Marks a navigation property (object or collection) as a reference to a searchable entity.
/// The referenced entity is identified by the property of the navigation object named as the entity key.
/// </summary>
/// <param name="entity">Searchable entity document type (<see cref="SearchableDocumentAttribute"/> with a key).</param>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class ReferencesAttribute(Type entity) : Attribute
{
    public Type Entity { get; } = entity;
}
