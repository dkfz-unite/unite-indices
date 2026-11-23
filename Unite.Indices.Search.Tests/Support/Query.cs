using System.Text.Json.Nodes;

namespace Unite.Indices.Search.Tests.Support;

/// <summary>
/// Reads conditions from an Elasticsearch request (JSON).
/// </summary>
public static class Query
{
    /// <summary>
    /// Values of all "terms" conditions on the field: required (outside "must_not") or excluded (inside "must_not").
    /// </summary>
    public static (List<HashSet<string>> Required, List<HashSet<string>> Excluded) Terms(string request, string field)
    {
        var required = new List<HashSet<string>>();
        var excluded = new List<HashSet<string>>();

        void Walk(JsonNode node, bool negated)
        {
            switch (node)
            {
                case JsonObject obj:
                    foreach (var (name, value) in obj)
                    {
                        if (name == "terms" && value is JsonObject terms && terms[field] is JsonArray values)
                            (negated ? excluded : required).Add(values.Select(v => v.ToString()).ToHashSet());
                        else if (value != null)
                            Walk(value, negated ^ name == "must_not");
                    }
                    break;
                case JsonArray array:
                    foreach (var item in array.Where(item => item != null))
                        Walk(item, negated);
                    break;
            }
        }

        Walk(JsonNode.Parse(request), false);

        return (required, excluded);
    }

    /// <summary>
    /// The request has exactly one required "terms" condition on the field, with these values.
    /// </summary>
    public static void Requires(string request, string field, params string[] values)
    {
        var (required, _) = Terms(request, field);

        Assert.True(required.Count == 1, $"Expected one required terms condition on '{field}', found {required.Count}: {request}");
        Assert.Equal(values.ToHashSet(), required[0]);
    }

    /// <summary>
    /// The request has exactly one excluded ("must_not") "terms" condition on the field, with these values.
    /// </summary>
    public static void Excludes(string request, string field, params string[] values)
    {
        var (_, excluded) = Terms(request, field);

        Assert.True(excluded.Count == 1, $"Expected one excluded terms condition on '{field}', found {excluded.Count}: {request}");
        Assert.Equal(values.ToHashSet(), excluded[0]);
    }

    /// <summary>
    /// The request has no "terms" condition on the field.
    /// </summary>
    public static void HasNoTerms(string request, string field)
    {
        var (required, excluded) = Terms(request, field);

        Assert.True(required.Count == 0 && excluded.Count == 0, $"Expected no terms condition on '{field}': {request}");
    }
}
