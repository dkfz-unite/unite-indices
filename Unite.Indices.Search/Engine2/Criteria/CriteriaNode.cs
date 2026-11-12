using System.Text.Json.Serialization;
using Unite.Indices.Search.Engine2.Criteria.Properties;

namespace Unite.Indices.Search.Engine2.Criteria;

/// <summary>
/// Node of a criteria tree. A plain node combines its child nodes with <see cref="Operator"/>;
/// a <see cref="PropertyCriteria"/> is a condition on its own and is combined with its child nodes, if any.
/// <see cref="Negate"/> applies to the whole node including its children.
/// </summary>
/// <remarks>
/// In JSON the node type is given by the "kind" discriminator, which must be the first property of the object.
/// A node without "kind" is a plain node.
/// </remarks>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(CriteriaNode))]
[JsonDerivedType(typeof(ValueCriteria<string>), "value-string")]
[JsonDerivedType(typeof(ValueCriteria<int>), "value-int")]
[JsonDerivedType(typeof(ValueCriteria<long>), "value-long")]
[JsonDerivedType(typeof(ValueCriteria<double>), "value-double")]
[JsonDerivedType(typeof(ValueCriteria<bool>), "value-bool")]
[JsonDerivedType(typeof(ValueCriteria<DateTime>), "value-date")]
[JsonDerivedType(typeof(RangeCriteria<int>), "range-int")]
[JsonDerivedType(typeof(RangeCriteria<long>), "range-long")]
[JsonDerivedType(typeof(RangeCriteria<double>), "range-double")]
[JsonDerivedType(typeof(ExistsCriteria), "exists")]
[JsonDerivedType(typeof(SimilarityCriteria), "similarity")]
public class CriteriaNode
{
    public bool Negate { get; set; }

    public LogicalOperator Operator { get; set; } = LogicalOperator.And;
    public List<CriteriaNode> Nodes { get; set; }
}
