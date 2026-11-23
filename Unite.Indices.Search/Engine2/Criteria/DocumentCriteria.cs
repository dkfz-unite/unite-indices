using System.Text.Json;
using System.Text.Json.Serialization;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.Criteria;

/// <summary>
/// Criteria of a document type: a tree of documents to include and a tree of documents to exclude.
/// For other documents than the search target: target documents related to documents matching <see cref="Include"/> are required,
/// target documents related to documents matching <see cref="Exclude"/> are excluded (e.g. images with SMs in TP53, but without SMs in IDH1).
/// For the search target: its documents have to match <see cref="Include"/> and must not match <see cref="Exclude"/>.
/// </summary>
/// <remarks>
/// Both are independent of <see cref="CriteriaNode.Negate"/>, which only negates conditions within a tree:
/// excluding a negated root excludes target documents related to documents not matching its nodes.
/// </remarks>
public abstract class DocumentCriteria
{
    /// <summary>
    /// Criteria of documents to include, null if there are none.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CriteriaGroup Include { get; set; }

    /// <summary>
    /// Criteria of documents to exclude, null if there are none.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public CriteriaGroup Exclude { get; set; }
}

/// <summary>
/// Criteria of documents of type <typeparamref name="TDocument"/>.
/// </summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public class DocumentCriteria<TDocument> : DocumentCriteria, IJsonOnDeserialized where TDocument : class
{
    /// <summary>
    /// Resolves field names of all property criteria in both trees to typed fields of the document.
    /// </summary>
    /// <exception cref="ArgumentException">A node has no kind, a criteria has no field, an unknown field or values not matching the field type.</exception>
    public void Resolve()
    {
        if (Include != null)
            SearchFields<TDocument>.Resolve(Include);

        if (Exclude != null)
            SearchFields<TDocument>.Resolve(Exclude);
    }

    void IJsonOnDeserialized.OnDeserialized()
    {
        try
        {
            Resolve();
        }
        catch (ArgumentException exception)
        {
            throw new JsonException(exception.Message, exception);
        }
    }
}
