using System.Collections;
using System.Globalization;
using Unite.Indices.Search.Engine2.Access;
using Unite.Indices.Search.Engine2.Fields;
using Unite.Indices.Search.Tests.Support;

namespace Unite.Indices.Search.Tests.Integration;

/// <summary>
/// Criteria JSON and Engine2 searches by document name.
/// </summary>
public static class Searches
{
    public record Found(long Total, string[] Ids);


    public static string Text(string property, params string[] values) =>
        $$"""{ "kind": "value-string", "property": "{{property}}", "values": [{{string.Join(", ", values.Select(value => $"\"{value}\""))}}] }""";

    public static string Bool(string property, bool value) =>
        $$"""{ "kind": "value-bool", "property": "{{property}}", "values": [{{(value ? "true" : "false")}}] }""";

    public static string Exists(string property) =>
        $$"""{ "kind": "exists", "property": "{{property}}" }""";

    public static string Range(string kind, string property, double? from = null, double? to = null) =>
        $$"""{ "kind": "range-{{kind}}", "property": "{{property}}"{{Bound("from", from)}}{{Bound("to", to)}} }""";

    private static string Bound(string name, double? value) =>
        value == null ? "" : $", \"{name}\": {value.Value.ToString(CultureInfo.InvariantCulture)}";

    /// <summary>
    /// Criteria of a document: include and exclude groups of nodes (null: no group).
    /// </summary>
    public static string Document(string name, string[] include = null, string[] exclude = null)
    {
        var groups = new[] { ("include", include), ("exclude", exclude) }
            .Where(group => group.Item2 != null)
            .Select(group => $$"""
                "{{group.Item1}}": { "nodes": [ {{string.Join(", ", group.Item2)}} ] }
                """);

        return $$""" "{{name}}": { {{string.Join(", ", groups)}} } """;
    }

    public static string Criteria(params string[] documents) => "{" + string.Join(", ", documents) + "}";


    public static async Task<Found> Search(IServiceProvider services, string document, string criteria, SearchScope scope, string term = null)
    {
        var type = SearchDocuments.Find(document).Type;
        var service = Activator.CreateInstance(typeof(Engine2.SearchService<>).MakeGenericType(type), [services]);
        var task = (Task)service.GetType().GetMethod("Search").Invoke(service, [FakeIndices.Criteria(criteria), scope, 0, 100, term]);

        await task;

        var result = task.GetType().GetProperty("Result").GetValue(task);
        var total = (long)result.GetType().GetProperty("Total").GetValue(result);
        var rows = (IEnumerable)result.GetType().GetProperty("Rows").GetValue(result);

        return new Found(total, Ids(rows));
    }

    /// <summary>
    /// IDs of the documents, in their order.
    /// </summary>
    public static string[] Ids(IEnumerable rows) =>
        rows.Cast<object>().Select(row => Convert.ToString(row.GetType().GetProperty("Id").GetValue(row), CultureInfo.InvariantCulture)).ToArray();
}
