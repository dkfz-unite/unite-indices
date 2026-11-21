using System.Text.Json.Serialization;

namespace Unite.Indices.Search.Engine2.Criteria;

/// <summary>
/// Combines child nodes with <see cref="Operator"/>.
/// </summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public class CriteriaGroup : CriteriaNode
{
    public LogicalOperator Operator { get; set; } = LogicalOperator.And;
    public List<CriteriaNode> Nodes { get; set; }
}
