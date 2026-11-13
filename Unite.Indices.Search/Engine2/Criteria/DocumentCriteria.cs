using System.Text.Json;
using System.Text.Json.Serialization;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.Criteria;

/// <summary>
/// Root criteria group for documents of type <typeparamref name="TDocument"/>.
/// </summary>
[JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
public class DocumentCriteria<TDocument> : CriteriaGroup, IJsonOnDeserialized where TDocument : class
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
