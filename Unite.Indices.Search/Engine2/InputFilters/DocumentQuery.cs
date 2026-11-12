namespace Unite.Indices.Search.Engine2.InputFilters;

public interface IDocumentQuery
{
    public FilterGroup FilterGroup { get; }
}

public class DocumentQuery<TDocument> : IDocumentQuery
{
    public FilterGroup FilterGroup { get; set; }
}
