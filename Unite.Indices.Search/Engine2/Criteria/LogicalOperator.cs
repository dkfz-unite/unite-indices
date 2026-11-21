using System.Text.Json.Serialization;

namespace Unite.Indices.Search.Engine2.Criteria;

[JsonConverter(typeof(JsonStringEnumConverter<LogicalOperator>))]
public enum LogicalOperator
{
    And,
    Or
}
