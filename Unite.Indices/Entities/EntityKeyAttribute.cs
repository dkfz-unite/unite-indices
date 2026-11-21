namespace Unite.Indices.Entities;

/// <summary>
/// Marks the key property of an entity (e.g. donor or specimen id).
/// A searchable document with a key property is an entity other documents can refer to;
/// navigation objects referring to an entity are identified by their key property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class EntityKeyAttribute : Attribute
{
}
