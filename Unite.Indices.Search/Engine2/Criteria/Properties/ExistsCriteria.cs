using System.Text.Json.Serialization;
namespace Unite.Indices.Search.Engine2.Criteria.Properties;

/// <summary>
/// Property has a value. Negate to find documents without a value.
/// </summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public class ExistsCriteria : PropertyCriteria
{
    public override TResult Accept<TResult>(IPropertyCriteriaVisitor<TResult> visitor) => visitor.Visit(this);
}
