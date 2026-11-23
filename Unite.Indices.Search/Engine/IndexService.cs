using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using Nest;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine;

public abstract class IndexService<T> : IIndexService<T>
    where T : class
{
    protected readonly IElasticClient _client;

    protected abstract string Collection { get; }

    /// <summary>
    /// Buckets per composite aggregation page.
    /// </summary>
    protected virtual int KeysPageSize => 1000;


    /// <summary>
    /// Uses the given client (e.g. with an in-memory connection in tests).
    /// </summary>
    protected IndexService(IElasticClient client)
    {
        _client = client;
    }

    public IndexService(IElasticOptions options)
    {
        var host = new Uri(options.Host);

        var settings = new ConnectionSettings(host)
                .BasicAuthentication(options.User, options.Password)
                .DisableAutomaticProxyDetection()
                .DefaultIndex(Collection)
#if DEBUG
                .PrettyJson()
                .DisableDirectStreaming()
                .OnRequestCompleted(details =>
                {
                    Console.WriteLine($"{details.HttpMethod} {details.Uri}");

                    if (details.RequestBodyInBytes != null)
                    {
                        Console.WriteLine("REQUEST:");
                        Console.WriteLine(Encoding.UTF8.GetString(details.RequestBodyInBytes));
                    }

                    if (details.ResponseBodyInBytes != null)
                    {
                        Console.WriteLine("RESPONSE:");
                        Console.WriteLine(Encoding.UTF8.GetString(details.ResponseBodyInBytes));
                    }

                    Console.WriteLine(new string('-', 80));
                })
#endif
            ;

        _client = new ElasticClient(settings);
    }


    public virtual async Task<T> Get(GetQuery<T> query)
    {
        var request = query.GetRequest();

        var response = await _client.GetAsync<T>(request);

        if (response.IsValid)
        {
            return response.Source;
        }
        else
        {
            return null;
        }
    }

    public virtual async Task<SearchResult<T>> Search(SearchQuery<T> query)
    {
        var request = query.GetRequest();

        var response = await _client.SearchAsync<T>(request);

        if (response.IsValid)
        {
            return new SearchResult<T>()
            {
                Total = response.Total,
                Rows = response.Documents,
                Aggregations = query.Aggregations.ToDictionary(
                    name => name, 
                    name => response.GetTermsAggregationData<T>(name)
                )
            };
        }
        else
        {
            return new SearchResult<T>();
        }
    }

    public virtual async Task<IReadOnlyDictionary<string, HashSet<string>>> CollectKeys(SearchQuery<T> query, IReadOnlyDictionary<string, LambdaExpression> properties)
    {
        var request = query.GetRequest();
        request.From = 0;
        request.Size = 0;

        var keys = properties.Keys.ToDictionary(name => name, _ => new HashSet<string>());
        var pending = properties.Keys.ToDictionary(name => name, _ => (CompositeKey)null);

        while (pending.Count > 0)
        {
            var aggregations = new AggregationDictionary();

            foreach (var (name, after) in pending)
            {
                aggregations.Add(name, new CompositeAggregation(name)
                {
                    Sources = [new TermsCompositeAggregationSource("key") { Field = new Field(properties[name]) }],
                    Size = KeysPageSize,
                    After = after
                });
            }

            request.Aggregations = aggregations;

            var response = await _client.SearchAsync<T>(request);

            // A failed request must not look like "no keys": that would drop required keys silently or skip exclusions.
            if (!response.IsValid)
                throw new InvalidOperationException($"Collecting keys from '{Collection}' failed: {response.DebugInformation}");

            foreach (var name in pending.Keys.ToList())
            {
                var aggregate = response.Aggregations.Composite(name);

                foreach (var bucket in aggregate.Buckets)
                    keys[name].Add(Convert.ToString(bucket.Key["key"], CultureInfo.InvariantCulture));

                if (aggregate.Buckets.Count < KeysPageSize || aggregate.AfterKey == null)
                    pending.Remove(name);
                else
                    pending[name] = aggregate.AfterKey;
            }
        }

        return keys;
    }

    public virtual async Task<IEnumerable<IHit<T>>> Search(string term)
    {
        var request = new SearchRequest<T>
        {
            Query = Query<T>.MultiMatch(d => d.Query(term))
        };

        var response = await _client.SearchAsync<T>(request);
        

        if (response.IsValid)
        {
            return response.Hits;
        }
        else
        {
            return new List<IHit<T>>();
        }
    }
}
