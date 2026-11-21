using System.Text.Json.Serialization;
using Unite.Indices.Search.Engine2.Criteria.Properties;

namespace Unite.Indices.Search.Engine2.Criteria;

/// <summary>
/// Node of a criteria tree: either a <see cref="CriteriaGroup"/> combining child nodes,
/// or a <see cref="PropertyCriteria"/> with a condition on a single property.
/// <see cref="Negate"/> applies to the whole node.
/// </summary>
/// <remarks>
/// In JSON the node type is given by the "kind" discriminator, which must be the first property of the object.
/// Nodes without "kind" are read as plain nodes and rejected on resolution.
/// </remarks>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(CriteriaNode))]
[JsonDerivedType(typeof(CriteriaGroup), "group")]
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
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public class CriteriaNode
{
    public bool Negate { get; set; }

    /// <summary>
    /// Only derived nodes can be created; the JSON serializer creates plain nodes for input without "kind".
    /// </summary>
    [JsonConstructor]
    private protected CriteriaNode()
    {
    }
}
