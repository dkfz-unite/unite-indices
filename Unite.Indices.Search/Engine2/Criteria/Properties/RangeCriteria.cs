using System.Numerics;

namespace Unite.Indices.Search.Engine2.Criteria.Properties;

/// <summary>
/// Numeric range (inclusive). Either bound can be omitted.
/// </summary>
public class RangeCriteria<TValue> : PropertyCriteria where TValue : struct, INumber<TValue>
{
    public TValue? From { get; set; }
    public TValue? To { get; set; }

    public override TResult Accept<TResult>(IPropertyCriteriaVisitor<TResult> visitor) => visitor.Visit(this);
}
