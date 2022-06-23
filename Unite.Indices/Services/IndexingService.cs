﻿using System.Linq.Expressions;
using Nest;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Indices.Services;

public abstract class IndexingService<T> : IIndexingService<T>
    where T : class
{
    protected readonly IElasticClient _client;

    protected abstract string DefaultIndex { get; }

    protected abstract Expression<Func<T, object>> IdProperty { get; }

    public IndexingService(IElasticOptions options)
    {
        var host = new Uri(options.Host);

        var settings = new ConnectionSettings(host)
            .DisableAutomaticProxyDetection()
            .BasicAuthentication(options.User, options.Password)
            .DefaultMappingFor<T>(map => map
                .IndexName(DefaultIndex)
                .IdProperty(IdProperty)
            );

        _client = new ElasticClient(settings);
    }

    public virtual async Task IndexOne(T index)
    {
        var response = await _client.UpdateAsync<T>(index, update => update
            .Doc(index)
            .DocAsUpsert()
        );

        if (!response.IsValid)
        {
            throw response.OriginalException;
        }
    }

    public virtual async Task IndexMany(IEnumerable<T> indices)
    {
        var response = await _client.BulkAsync(bulk => bulk
            .UpdateMany(indices, (update, index) => update
                .Doc(index)
                .DocAsUpsert()
            )
        );

        if (!response.IsValid)
        {
            throw response.OriginalException;
        }
    }

    public virtual async Task UpdateMapping()
    {
        var existsResponse = await _client.Indices.ExistsAsync(DefaultIndex);

        if (existsResponse.OriginalException != null)
        {
            throw existsResponse.OriginalException;
        }

        if (existsResponse.Exists)
        {
            var updateMappingResponse = await _client.Indices.PutMappingAsync<T>(mapping =>
                mapping.AutoMap()
            );

            if (updateMappingResponse.OriginalException != null)
            {
                throw updateMappingResponse.OriginalException;
            }
        }
        else
        {
            var createIndexResponse = await _client.Indices.CreateAsync(DefaultIndex, request =>
                request.Map<T>(mapping => mapping.AutoMap())
            );

            if (createIndexResponse.OriginalException != null)
            {
                throw createIndexResponse.OriginalException;
            }
        }
    }
}
