using System.Text.Json.Serialization;
namespace Unite.Indices.Search.Engine2.Criteria.Properties;

/// <summary>
/// Property equals any of the values.
/// </summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public class ValueCriteria<TValue> : PropertyCriteria
{
    public List<TValue> Values { get; set; }

    public override TResult Accept<TResult>(IPropertyCriteriaVisitor<TResult> visitor) => visitor.Visit(this);
}
