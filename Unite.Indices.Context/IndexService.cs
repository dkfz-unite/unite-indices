using System.Linq.Expressions;
using Nest;
using Unite.Indices.Context.Configuration.Options;

namespace Unite.Indices.Context;

public abstract class IndexService<T> : IIndexService<T> 
    where T : class
{
    protected readonly IElasticClient _client;

    protected abstract string Collection { get; }    
    protected abstract Expression<Func<T, object>> Identifier { get; }


    public IndexService(IElasticOptions options)
    {
        var host = new Uri(options.Host);

        var settings = new ConnectionSettings(host)
            .DisableAutomaticProxyDetection()
            .BasicAuthentication(options.User, options.Password)
            .EnableApiVersioningHeader()
            .DefaultMappingFor<T>(map => map
                .IndexName(Collection)
                .IdProperty(Identifier)
            );

        _client = new ElasticClient(settings);
    }


    public virtual async Task Add(T element)
    {
        var response = await _client.UpdateAsync<T>(element, update => update
            .Doc(element)
            .DocAsUpsert()
        );

        HandleResponseErrors(response);
    }

    public virtual async Task AddRange(IEnumerable<T> elements)
    {
        var response = await _client.BulkAsync(bulk => bulk
            .UpdateMany(elements, (update, element) => update
                .Doc(element)
                .DocAsUpsert()
            )
        );

        HandleResponseErrors(response);
    }


    public virtual async Task UpdateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        HandleResponseErrors(existsResponse);

        if (existsResponse.Exists)
        {
            var updateMappingResponse = await _client.Indices.PutMappingAsync<T>(mapping =>
                mapping.AutoMap()
            );

            HandleResponseErrors(updateMappingResponse);
        }
        else
        {
            var createIndexResponse = await _client.Indices.CreateAsync(Collection, request =>
                request.Map<T>(mapping => mapping.AutoMap())
            );

            HandleResponseErrors(createIndexResponse);
        }
    }

    public virtual async Task DeleteIndex()
    {
        var response = await _client.Indices.DeleteAsync(Collection);

        HandleResponseErrors(response);
    }


    private static void HandleResponseErrors(IResponse response)
    {
        if (!response.IsValid)
        {
            throw response.OriginalException ?? new Exception(response.DebugInformation);
        }
    }
}
