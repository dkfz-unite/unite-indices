namespace Unite.Indices.Search.Engine2.Criteria.Properties;

/// <summary>
/// Condition on a single document property, given as a dot-separated path (e.g. "specimens.id").
/// </summary>
public abstract class PropertyCriteria : CriteriaNode
{
    public string Property { get; set; }

    public abstract TResult Accept<TResult>(IPropertyCriteriaVisitor<TResult> visitor);
}
