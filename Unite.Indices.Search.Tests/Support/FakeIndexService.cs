using Elasticsearch.Net;
using Nest;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;

namespace Unite.Indices.Search.Tests.Support;

/// <summary>
/// In-memory index service: records the requests and answers terms aggregations with configured keys.
/// </summary>
public class FakeIndexService<T> : IIndexService<T> where T : class
{
    private static readonly IElasticClient _client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DisableDirectStreaming());

    private readonly Queue<Dictionary<string, string[]>> _responses = new();
    private readonly Dictionary<string, string[]> _defaults = new();

    /// <summary>
    /// Requests sent to the index, as JSON.
    /// </summary>
    public List<string> Requests { get; } = [];


    /// <summary>
    /// Keys returned for the aggregation (e.g. "specimens_keys") by every request without a queued response.
    /// </summary>
    public FakeIndexService<T> Returns(string aggregation, params string[] keys)
    {
        _defaults[aggregation] = keys;
        return this;
    }

    /// <summary>
    /// Keys returned by the next request (one call per request, in order).
    /// </summary>
    public FakeIndexService<T> ReturnsNext(params (string Aggregation, string[] Keys)[] aggregations)
    {
        _responses.Enqueue(aggregations.ToDictionary(entry => entry.Aggregation, entry => entry.Keys));
        return this;
    }

    public Task<SearchResult<T>> Search(SearchQuery<T> query)
    {
        Requests.Add(_client.RequestResponseSerializer.SerializeToString(query.GetRequest()));

        var response = _responses.Count > 0 ? _responses.Dequeue() : _defaults;

        return Task.FromResult(new SearchResult<T>
        {
            Total = 1,
            Aggregations = query.Aggregations.ToDictionary(
                name => name,
                name => (IDictionary<string, long>)(response.TryGetValue(name, out var keys) ? keys : []).ToDictionary(key => key, key => 1L))
        });
    }

    public Task<T> Get(GetQuery<T> query) => throw new NotSupportedException();

    public Task<IEnumerable<IHit<T>>> Search(string query) => throw new NotSupportedException();
}
