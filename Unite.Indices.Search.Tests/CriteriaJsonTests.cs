using System.Text.Json;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Criteria.Properties;
using Unite.Indices.Search.Engine2.Fields;
using Unite.Indices.Search.Tests.Support;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;

namespace Unite.Indices.Search.Tests;

/// <summary>
/// Criteria JSON: shape, typed values, field resolution and validation while reading.
/// </summary>
public class CriteriaJsonTests
{
    [Fact]
    public void Reads_include_and_exclude_trees_with_typed_criteria()
    {
        var criteria = FakeIndices.Criteria("""
        { "sms": {
            "include": { "operator": "or", "nodes": [
              { "kind": "value-string", "property": "type", "values": ["SNV"] },
              { "kind": "group", "negate": true, "nodes": [ { "kind": "range-int", "property": "start", "from": 100, "to": 200 } ] } ] },
            "exclude": { "nodes": [ { "kind": "exists", "property": "affectedFeatures.effects.impact" } ] } } }
        """);

        var sms = criteria.Get<SmIndex>();

        Assert.Equal(LogicalOperator.Or, sms.Include.Operator);
        var type = Assert.IsType<ValueCriteria<string>>(sms.Include.Nodes[0]);
        Assert.Equal(["SNV"], type.Values);
        Assert.IsType<SearchField<SmIndex, string>>(type.Property);

        var group = Assert.IsType<CriteriaGroup>(sms.Include.Nodes[1]);
        Assert.True(group.Negate);
        var range = Assert.IsType<RangeCriteria<int>>(group.Nodes[0]);
        Assert.Equal(100, range.From);
        Assert.Equal(200, range.To);

        var exists = Assert.IsType<ExistsCriteria>(sms.Exclude.Nodes[0]);
        Assert.Equal("affectedFeatures.effects.impact", exists.Property.Name);
    }

    [Fact]
    public void Round_trip_keeps_the_shape()
    {
        var json = """{"sms":{"include":{"operator":"Or","nodes":[{"kind":"value-string","values":["SNV"],"property":"type","negate":false}],"negate":false}}}""";

        Assert.Equal(json, JsonSerializer.Serialize(FakeIndices.Criteria(json), FakeIndices.JsonOptions));
    }

    [Theory]
    [InlineData("unknown document", """{ "variants": { "include": { "nodes": [] } } }""", "is not a searchable document")]
    [InlineData("document given twice", """{ "sms": { "include": { "nodes": [] } }, "SMS": { "include": { "nodes": [] } } }""", "given twice")]
    [InlineData("criteria without include/exclude", """{ "sms": { "nodes": [] } }""", "could not be mapped")]
    [InlineData("typo", """{ "sms": { "inclde": { "nodes": [] } } }""", "could not be mapped")]
    [InlineData("unknown field", """{ "sms": { "include": { "nodes": [ { "kind": "exists", "property": "stats.donors" } ] } } }""", "is not a searchable property")]
    [InlineData("access data of projects", """{ "projects": { "include": { "nodes": [ { "kind": "exists", "property": "users.userId" } ] } } }""", "is not a searchable property")]
    [InlineData("value type mismatch", """{ "sms": { "include": { "nodes": [ { "kind": "value-int", "property": "type", "values": [1] } ] } } }""", "criteria values are Int32")]
    [InlineData("range on text", """{ "sms": { "include": { "nodes": [ { "kind": "range-int", "property": "type", "from": 1 } ] } } }""", "range criteria need numbers")]
    [InlineData("wrong JSON value type", """{ "sms": { "include": { "nodes": [ { "kind": "value-int", "property": "start", "values": ["abc"] } ] } } }""", "could not be converted")]
    [InlineData("unknown kind", """{ "sms": { "include": { "nodes": [ { "kind": "value-guid", "property": "type", "values": [] } ] } } }""", "unrecognized type discriminator")]
    [InlineData("node without kind", """{ "sms": { "include": { "nodes": [ {} ] } } }""", "kind is required")]
    [InlineData("children on a leaf", """{ "sms": { "include": { "nodes": [ { "kind": "exists", "property": "type", "nodes": [] } ] } } }""", "could not be mapped")]
    [InlineData("missing property", """{ "sms": { "include": { "nodes": [ { "kind": "exists" } ] } } }""", "property is required")]
    public void Invalid_input_is_rejected_while_reading(string _, string json, string message)
    {
        var exception = Assert.ThrowsAny<JsonException>(() => FakeIndices.Criteria(json));

        Assert.Contains(message, exception.Message);
    }
}
