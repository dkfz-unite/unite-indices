using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.Criteria.Properties;

/// <summary>
/// Condition on a single searchable document property.
/// In JSON the property is given by its name (e.g. "specimens.id") and resolved to a typed field of the document on deserialization.
/// </summary>
public abstract class PropertyCriteria : CriteriaNode
{
    public SearchField Property { get; set; }

    public abstract TResult Accept<TResult>(IPropertyCriteriaVisitor<TResult> visitor);
}
