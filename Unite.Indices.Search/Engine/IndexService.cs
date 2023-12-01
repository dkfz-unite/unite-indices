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


    public IndexService(IElasticOptions options)
    {
        var host = new Uri(options.Host);

        var settings = new ConnectionSettings(host)
            .BasicAuthentication(options.User, options.Password)
            .DisableAutomaticProxyDetection()
            .DefaultIndex(Collection);

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
}
