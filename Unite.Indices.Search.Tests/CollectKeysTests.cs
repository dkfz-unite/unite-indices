using System.Linq.Expressions;
using System.Text;
using System.Text.Json.Nodes;
using Elasticsearch.Net;
using Nest;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;

namespace Unite.Indices.Search.Tests;

/// <summary>
/// Key collection of the index services: composite aggregations paged until all keys are read (no 1000-key limit).
/// </summary>
public class CollectKeysTests
{
    /// <summary>
    /// Connection answering requests with queued responses and recording the requests.
    /// </summary>
    private class QueuedConnection(params string[] responses) : InMemoryConnection
    {
        private readonly Queue<string> _responses = new(responses);

        public List<JsonNode> Requests { get; } = [];

        public override Task<TResponse> RequestAsync<TResponse>(RequestData requestData, CancellationToken cancellationToken)
        {
            using var stream = new MemoryStream();
            requestData.PostData.Write(stream, requestData.ConnectionSettings);
            Requests.Add(JsonNode.Parse(Encoding.UTF8.GetString(stream.ToArray())));

            return ReturnConnectionStatusAsync<TResponse>(requestData, cancellationToken, Encoding.UTF8.GetBytes(_responses.Dequeue()), 200, "application/json");
        }
    }

    private class DonorsIndexService(IElasticClient client, int pageSize) : IndexService<DonorIndex>(client)
    {
        protected override string Collection => "donors";
        protected override int KeysPageSize => pageSize;
    }

    private static string Response(params (string Name, int[] Keys, int? After)[] aggregations)
    {
        var parts = aggregations.Select(aggregation =>
            $$"""
            "{{aggregation.Name}}": {
              {{(aggregation.After == null ? "" : $$"""
              "after_key": { "key": {{aggregation.After}} },
              """)}}
              "buckets": [ {{string.Join(",", aggregation.Keys.Select(key => $$"""{ "key": { "key": {{key}} }, "doc_count": 1 }"""))}} ] }
            """);

        return $$"""{ "took": 1, "timed_out": false, "hits": { "total": { "value": 0, "relation": "eq" }, "hits": [] }, "aggregations": { {{string.Join(",", parts)}} } }""";
    }

    private static IElasticClient Client(IConnection connection) =>
        new ElasticClient(new ConnectionSettings(new SingleNodeConnectionPool(new Uri("http://localhost:9200")), connection).DefaultIndex("donors"));

    private static IReadOnlyDictionary<string, LambdaExpression> Keys(params (string Name, Expression<Func<DonorIndex, int>> Property)[] keys) =>
        keys.ToDictionary(key => key.Name, key => (LambdaExpression)key.Property);


    [Fact]
    public async Task Pages_until_a_page_is_not_full()
    {
        var connection = new QueuedConnection(
            Response(("donors_keys", [1, 2], 2)),
            Response(("donors_keys", [3, 4], 4)),
            Response(("donors_keys", [5], 5)));
        var service = new DonorsIndexService(Client(connection), pageSize: 2);

        var keys = await service.CollectKeys(new SearchQuery<DonorIndex>(), Keys(("donors_keys", donor => donor.Id)));

        Assert.Equal(["1", "2", "3", "4", "5"], keys["donors_keys"].Order().ToArray());
        Assert.Equal(3, connection.Requests.Count);
        Assert.Null(connection.Requests[0]["aggs"]["donors_keys"]["composite"]["after"]);
        Assert.Equal(2, (int)connection.Requests[1]["aggs"]["donors_keys"]["composite"]["after"]["key"]);
        Assert.Equal(4, (int)connection.Requests[2]["aggs"]["donors_keys"]["composite"]["after"]["key"]);
    }

    [Fact]
    public async Task Several_keys_share_requests_until_each_is_complete()
    {
        var connection = new QueuedConnection(
            Response(("donors_keys", [1, 2], 2), ("specimens_keys", [7], 7)),
            Response(("donors_keys", [3], 3)));
        var service = new DonorsIndexService(Client(connection), pageSize: 2);

        var keys = await service.CollectKeys(new SearchQuery<DonorIndex>(),
            Keys(("donors_keys", donor => donor.Id), ("specimens_keys", donor => donor.Specimens.First().Id)));

        Assert.Equal(["1", "2", "3"], keys["donors_keys"].Order().ToArray());
        Assert.Equal(["7"], keys["specimens_keys"].ToArray());
        Assert.Equal(2, connection.Requests.Count);
        Assert.NotNull(connection.Requests[0]["aggs"]["specimens_keys"]);
        Assert.Null(connection.Requests[1]["aggs"]["specimens_keys"]);
        Assert.Equal("specimens.id", (string)connection.Requests[0]["aggs"]["specimens_keys"]["composite"]["sources"][0]["key"]["terms"]["field"]);
    }

    [Fact]
    public async Task Failed_requests_are_errors_not_empty_keys()
    {
        var connection = new QueuedConnection("""{ "error": "boom", "status": 500 }""");
        var service = new DonorsIndexService(Client(connection), pageSize: 2);

        await Assert.ThrowsAnyAsync<Exception>(() => service.CollectKeys(new SearchQuery<DonorIndex>(), Keys(("donors_keys", donor => donor.Id))));
    }
}
