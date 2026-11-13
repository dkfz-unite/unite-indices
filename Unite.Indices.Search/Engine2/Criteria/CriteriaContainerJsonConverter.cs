using System.Text.Json;
using System.Text.Json.Serialization;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.Criteria;

/// <summary>
/// Reads and writes <see cref="CriteriaContainer"/> as an object with one property per searchable document.
/// </summary>
public class CriteriaContainerJsonConverter : JsonConverter<CriteriaContainer>
{
    public override CriteriaContainer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Criteria must be an object with criteria per document.");

        var container = new CriteriaContainer();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            var name = reader.GetString();

            var document = SearchDocuments.Find(name)
                ?? throw new JsonException($"'{name}' is not a searchable document.");

            if (container.Get(document.Type) != null)
                throw new JsonException($"Criteria for '{name}' are given twice.");

            reader.Read();

            // DocumentCriteria<T> of the document resolves and validates its fields on deserialization.
            var criteria = (CriteriaGroup)JsonSerializer.Deserialize(ref reader, document.CriteriaType, options);

            container.Set(document.Type, criteria);
        }

        return container;
    }

    public override void Write(Utf8JsonWriter writer, CriteriaContainer value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var documentType in value.DocumentTypes)
        {
            var document = SearchDocuments.Get(documentType);

            writer.WritePropertyName(document.Name);
            JsonSerializer.Serialize(writer, value.Get(documentType), document.CriteriaType, options);
        }

        writer.WriteEndObject();
    }
}
