using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Configuration.Extensions;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Access;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Tests.Integration;

/// <summary>
/// Disposable single-node Elasticsearch 7.17 in Docker (random host port, never a local development instance),
/// with the fixture dataset indexed by the index services of Unite.Indices.Context.
/// Without Docker the integration tests are skipped.
/// </summary>
public class ElasticFixture : IAsyncLifetime
{
    public const string Image = "docker.elastic.co/elasticsearch/elasticsearch:7.17.25";

    /// <summary>
    /// Buckets per composite aggregation page in Engine2 searches: small, so that key collection pages for real.
    /// </summary>
    public const int KeysPageSize = 2;

    private IContainer _container;

    public IElasticOptions Options { get; private set; }

    /// <summary>
    /// Engine2 services: index services with small key pages that fail on invalid responses, and the projects access service.
    /// </summary>
    public IServiceProvider Services { get; private set; }

    /// <summary>
    /// Why the tests are skipped, if they are.
    /// </summary>
    public string Unavailable { get; private set; }


    public async Task InitializeAsync()
    {
        try
        {
            _container = new ContainerBuilder()
                .WithImage(Image)
                .WithEnvironment("discovery.type", "single-node")
                .WithEnvironment("xpack.security.enabled", "false")
                .WithEnvironment("ES_JAVA_OPTS", "-Xms512m -Xmx512m")
                .WithPortBinding(9200, assignRandomHostPort: true)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(request => request
                    .ForPort(9200)
                    .ForPath("/_cluster/health")))
                .Build();

            await _container.StartAsync();
        }
        catch (Exception exception)
        {
            Unavailable = $"Docker with {Image} is not available: {exception.Message}";
            return;
        }

        Options = new ElasticOptions($"http://{_container.Hostname}:{_container.GetMappedPublicPort(9200)}");

        await Dataset.Index(Options);

        Services = new ServiceCollection()
            .AddSingleton(Options)
            .AddSingleton<IAccessService, ProjectsAccessService>()
            .AddSingleton(typeof(IIndexService<>), typeof(TestIndexService<>))
            .BuildServiceProvider();
    }

    public async Task DisposeAsync()
    {
        if (_container != null)
            await _container.DisposeAsync();
    }


    private record ElasticOptions(string Host) : IElasticOptions
    {
        public string User => "elastic";
        public string Password => "unused";
    }

    /// <summary>
    /// Index service of any searchable document, paging keys in small pages and failing on invalid responses
    /// (the engine services return empty results instead, which would hide wrong queries).
    /// </summary>
    private class TestIndexService<T>(IElasticOptions options) : IndexService<T>(options) where T : class
    {
        protected override string Collection => SearchDocuments.Find(typeof(T)).Name;

        protected override int KeysPageSize => ElasticFixture.KeysPageSize;

        public override async Task<SearchResult<T>> Search(SearchQuery<T> query)
        {
            var response = await _client.SearchAsync<T>(query.GetRequest());

            if (!response.IsValid)
                throw new InvalidOperationException($"Search of '{Collection}' failed: {response.DebugInformation}");

            return new SearchResult<T>
            {
                Total = response.Total,
                Rows = response.Documents,
                Aggregations = new Dictionary<string, IDictionary<string, long>>()
            };
        }
    }
}

[CollectionDefinition(nameof(ElasticCollection))]
public class ElasticCollection : ICollectionFixture<ElasticFixture>
{
}
