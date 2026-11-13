using System.Text.Json;
using System.Text.Json.Serialization;

namespace Unite.Indices.Search.Engine2.Fields;

/// <summary>
/// Reads and writes a <see cref="SearchField"/> as its name.
/// Read fields are resolved to document properties by <see cref="SearchFields{TDocument}.Resolve"/>.
/// </summary>
public class SearchFieldJsonConverter : JsonConverter<SearchField>
{
    public override SearchField Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("Criteria property must be a string.");

        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, SearchField value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Name);
    }
}
