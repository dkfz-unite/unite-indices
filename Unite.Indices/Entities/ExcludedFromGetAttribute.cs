namespace Unite.Indices.Entities;

/// <summary>
/// Marks a (large) property of a searchable document that is not returned when a single document is requested,
/// e.g. the specimens of a gene.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public class ExcludedFromGetAttribute : Attribute
{
}
