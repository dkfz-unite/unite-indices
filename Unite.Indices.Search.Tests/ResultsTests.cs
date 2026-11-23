using System.Text.Json.Nodes;
using Unite.Indices.Search.Engine2.Access;
using Unite.Indices.Search.Engine2.References;
using Unite.Indices.Search.Tests.Support;
using GeneExpressionIndex = Unite.Indices.Entities.Genes.GeneExpressionIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;

namespace Unite.Indices.Search.Tests;

/// <summary>
/// How results are returned: sort orders of the old services, free text, properties left out of single documents.
/// </summary>
public class ResultsTests
{
    private static JsonArray Sort(string request) => (JsonArray)JsonNode.Parse(request)["sort"];

    private static string Field(JsonNode sort) => ((JsonObject)sort).Single().Key;

    private static string Direction(JsonNode sort) => (string)((JsonObject)sort).Single().Value["order"];


    [Theory]
    [InlineData("projects", "stats.donors.number:desc")]
    [InlineData("donors", "stats.genes:desc")]
    [InlineData("images", "stats.genes:desc")]
    [InlineData("specimens", "stats.genes:desc")]
    [InlineData("genes", "stats.donors:desc")]
    [InlineData("proteins", "stats.donors:desc")]
    [InlineData("sms", "stats.donors:desc", "chromosomeI:asc", "start:asc")]
    [InlineData("cnvs", "stats.donors:desc", "chromosomeI:asc", "start:asc")]
    [InlineData("svs", "stats.donors:desc", "chromosomeI:asc", "start:asc")]
    public async Task Results_are_sorted_as_in_the_old_services(string document, params string[] orders)
    {
        var indices = new FakeIndices();
        var type = Engine2.Fields.SearchDocuments.Find(document).Type;
        var service = Activator.CreateInstance(typeof(Engine2.SearchService<>).MakeGenericType(type), [indices.Services]);
        var search = service.GetType().GetMethod("Search");

        await (Task)search.Invoke(service, [FakeIndices.Criteria("{}"), SearchScope.All, 0, 20, null]);

        Assert.Equal(orders, Sort(indices.RequestsOf(type).Single()).Select(sort => $"{Field(sort)}:{Direction(sort)}").ToArray());
    }

    [Fact]
    public async Task Free_text_searches_the_text_fields_of_the_target_leniently()
    {
        var indices = new FakeIndices();

        await indices.SearchService<GeneIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.All, term: "EGFR");

        var match = JsonNode.Parse(indices.Genes.Requests.Single())["query"]["bool"]["must"].AsArray()
            .Select(node => node["multi_match"]).Single(node => node != null);

        Assert.Equal("EGFR", (string)match["query"]);
        Assert.True((bool)match["lenient"]);
        var fields = match["fields"].AsArray().Select(field => (string)field).ToArray();
        Assert.Contains("symbol", fields);
        Assert.Contains("biotype", fields);
        Assert.DoesNotContain("start", fields);
    }

    [Fact]
    public async Task Free_text_needs_text_fields()
    {
        var indices = new FakeIndices();

        await Assert.ThrowsAsync<ArgumentException>(() =>
            indices.SearchService<GeneExpressionIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.All, term: "EGFR"));
    }

    [Fact]
    public async Task Get_leaves_out_large_properties()
    {
        var indices = new FakeIndices();
        indices.Sms.ReturnsRows([new SmIndex { Id = 1 }]);

        await indices.SearchService<SmIndex>().Get("1", SearchScope.All);

        var excludes = JsonNode.Parse(indices.Sms.Requests.Single())["_source"]["excludes"].AsArray().Select(field => (string)field);
        Assert.Equal(["specimens"], excludes);
    }

    [Fact]
    public async Task Search_returns_whole_documents_as_the_old_engine()
    {
        var indices = new FakeIndices();

        await indices.SearchService<SmIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.All);

        Assert.Null(JsonNode.Parse(indices.Sms.Requests.Single())["_source"]);
    }
}

[CollectionDefinition(nameof(SearchLimitsCollection), DisableParallelization = true)]
public class SearchLimitsCollection
{
}

/// <summary>
/// Key sets larger than the terms limit of Elasticsearch are split into several terms queries.
/// </summary>
[Collection(nameof(SearchLimitsCollection))]
public class TermsLimitTests : IDisposable
{
    private readonly int _limit = SearchLimits.MaxTermsPerQuery;

    public TermsLimitTests()
    {
        SearchLimits.MaxTermsPerQuery = 2;
    }

    public void Dispose()
    {
        SearchLimits.MaxTermsPerQuery = _limit;
    }


    [Fact]
    public async Task Required_keys_are_split_into_alternatives()
    {
        var indices = new FakeIndices();
        indices.Sms.Returns("specimens_keys", "1", "2", "3", "4", "5");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("""
            { "sms": { "include": { "nodes": [ { "kind": "value-string", "property": "type", "values": ["SNV"] } ] } } }
            """), SearchScope.All);

        var (required, excluded) = Query.Terms(indices.Images.Requests.Single(), "specimens.id");
        Assert.Empty(excluded);
        Assert.Equal(3, required.Count);
        Assert.All(required, values => Assert.True(values.Count <= 2));
        Assert.Equal(["1", "2", "3", "4", "5"], required.SelectMany(values => values).Order().ToArray());
        Assert.Contains("\"should\"", indices.Images.Requests.Single());
    }

    [Fact]
    public async Task Excluded_keys_are_split_and_excluded_together()
    {
        var indices = new FakeIndices();
        indices.Sms.Returns("specimens_keys", "1", "2", "3");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("""
            { "sms": { "exclude": { "nodes": [ { "kind": "value-string", "property": "type", "values": ["SNV"] } ] } } }
            """), SearchScope.All);

        var (required, excluded) = Query.Terms(indices.Images.Requests.Single(), "specimens.id");
        Assert.Empty(required);
        Assert.Equal(["1", "2", "3"], excluded.SelectMany(values => values).Order().ToArray());
    }

    [Fact]
    public async Task Access_keys_are_split_too()
    {
        var indices = new FakeIndices();
        indices.Projects.Returns("projects_keys", "1", "2", "3");
        indices.Donors.Returns("donors_keys", "10");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(7));

        Assert.Equal(["1", "2", "3"], Query.Terms(indices.Donors.Requests.Single(), "projects.id").Required.SelectMany(values => values).Order().ToArray());
    }
}
