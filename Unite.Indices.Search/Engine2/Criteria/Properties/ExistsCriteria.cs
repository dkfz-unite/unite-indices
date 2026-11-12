namespace Unite.Indices.Search.Engine2.Criteria.Properties;

/// <summary>
/// Property has a value. Negate to find documents without a value.
/// </summary>
public class ExistsCriteria : PropertyCriteria
{
    public override TResult Accept<TResult>(IPropertyCriteriaVisitor<TResult> visitor) => visitor.Visit(this);
}
