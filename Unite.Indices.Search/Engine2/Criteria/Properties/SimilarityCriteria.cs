namespace Unite.Indices.Search.Engine2.Criteria.Properties;

/// <summary>
/// Full-text match of a string property against any of the values.
/// </summary>
public class SimilarityCriteria : PropertyCriteria
{
    public List<string> Values { get; set; }

    public override TResult Accept<TResult>(IPropertyCriteriaVisitor<TResult> visitor) => visitor.Visit(this);
}
