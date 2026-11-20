using System.Text.Json;
using System.Text.Json.Serialization;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.Criteria;

/// <summary>
/// Root criteria group of a document type.
/// </summary>
public abstract class DocumentCriteria : CriteriaGroup
{
    /// <summary>
    /// The criteria exclude: target documents related to documents matching the criteria are excluded
    /// (e.g. images without SMs of type SNV), instead of being required. Only for criteria of documents other than the search target.
    /// </summary>
    /// <remarks>
    /// Independent of <see cref="CriteriaNode.Negate"/>: a negated root still selects the documents not matching its nodes,
    /// e.g. with both set, images without SMs other than SNVs.
    /// </remarks>
    public bool IsExclusion { get; set; }
}

/// <summary>
/// Root criteria group for documents of type <typeparamref name="TDocument"/>.
/// </summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public class DocumentCriteria<TDocument> : DocumentCriteria, IJsonOnDeserialized where TDocument : class
{
    /// <summary>
    /// Resolves field names of all property criteria to typed fields of the document.
    /// </summary>
    /// <exception cref="ArgumentException">A node has no kind, a criteria has no field, an unknown field or values not matching the field type.</exception>
    public void Resolve()
    {
        SearchFields<TDocument>.Resolve(this);
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
