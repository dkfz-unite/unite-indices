using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nest;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Indices.Services
{
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

        public virtual async void IndexOne(T index)
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

        public virtual async void IndexMany(IEnumerable<T> indices)
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
    }
}
