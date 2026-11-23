using System.Linq.Expressions;
using Elasticsearch.Net;
using Nest;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;

namespace Unite.Indices.Search.Tests.Support;

/// <summary>
/// In-memory index service: records the requests, answers key collections with configured keys and searches with configured rows.
/// </summary>
public class FakeIndexService<T> : IIndexService<T> where T : class
{
    private static readonly IElasticClient _client = new ElasticClient(new ConnectionSettings(new InMemoryConnection()).DisableDirectStreaming());

    private readonly Queue<Dictionary<string, string[]>> _responses = new();
    private readonly Dictionary<string, string[]> _defaults = new();
    private readonly Queue<T[]> _rows = new();

    /// <summary>
    /// Requests sent to the index, as JSON.
    /// </summary>
    public List<string> Requests { get; } = [];

    /// <summary>
    /// Properties of collected keys per key collection, by name (e.g. "genes_keys": "document => document.AffectedFeatures.First().Gene.Id").
    /// </summary>
    public List<Dictionary<string, string>> Collected { get; } = [];


    /// <summary>
    /// Keys returned for the aggregation (e.g. "specimens_keys") by every request without a queued response.
    /// </summary>
    public FakeIndexService<T> Returns(string aggregation, params string[] keys)
    {
        _defaults[aggregation] = keys;
        return this;
    }

    /// <summary>
    /// Rows returned by the next searches (one page per search, in order).
    /// </summary>
    public FakeIndexService<T> ReturnsRows(params T[][] pages)
    {
        foreach (var page in pages)
            _rows.Enqueue(page);

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

        var rows = _rows.Count > 0 ? _rows.Dequeue() : [];

        return Task.FromResult(new SearchResult<T>
        {
            Total = rows.Length,
            Rows = rows,
            Aggregations = query.Aggregations.ToDictionary(
                name => name,
                name => (IDictionary<string, long>)(response.TryGetValue(name, out var keys) ? keys : []).ToDictionary(key => key, key => 1L))
        });
    }

    public Task<IReadOnlyDictionary<string, HashSet<string>>> CollectKeys(SearchQuery<T> query, IReadOnlyDictionary<string, LambdaExpression> properties)
    {
        Requests.Add(_client.RequestResponseSerializer.SerializeToString(query.GetRequest()));
        Collected.Add(properties.ToDictionary(entry => entry.Key, entry => entry.Value.ToString()));

        var response = _responses.Count > 0 ? _responses.Dequeue() : _defaults;

        return Task.FromResult<IReadOnlyDictionary<string, HashSet<string>>>(properties.Keys.ToDictionary(
            name => name,
            name => (response.TryGetValue(name, out var keys) ? keys : []).ToHashSet()));
    }

    public Task<T> Get(GetQuery<T> query) => throw new NotSupportedException();

    public Task<IEnumerable<IHit<T>>> Search(string query) => throw new NotSupportedException();
}
