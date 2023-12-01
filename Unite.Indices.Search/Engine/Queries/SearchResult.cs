namespace Unite.Indices.Search.Engine.Queries;

public class SearchResult<T> where T : class
{
    public long Total { get; set; }
    public IEnumerable<T> Rows { get; set; }
    public IDictionary<string, IDictionary<string, long>> Aggregations { get; set; }

    public SearchResult()
    {
        Total = 0;
        Rows = Enumerable.Empty<T>();
        Aggregations = new Dictionary<string, IDictionary<string, long>>();
    }
}
