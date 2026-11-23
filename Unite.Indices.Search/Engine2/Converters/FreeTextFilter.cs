using Nest;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.Converters;

/// <summary>
/// Free-text search in the searchable text fields of the document. Lenient: values not matching a field's format are ignored.
/// </summary>
internal sealed class FreeTextFilter<TDocument>(string text) : IFilter<TDocument> where TDocument : class
{
    public string Name => "FreeText";
    public bool Not { get; set; }
    public bool IsEmpty => string.IsNullOrWhiteSpace(text);

    public QueryContainer CreateQuery()
    {
        var fields = SearchResults<TDocument>.TextFields.Select(field => new Field(field.Property)).ToArray();

        return Query<TDocument>.MultiMatch(query => query
            .Query(text)
            .Fields(fields)
            .Lenient());
    }
}
