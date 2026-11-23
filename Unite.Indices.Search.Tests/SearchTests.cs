using Unite.Indices.Search.Engine2.Access;
using Unite.Indices.Search.Tests.Support;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;

namespace Unite.Indices.Search.Tests;

/// <summary>
/// Complete searches against fake indices: the requests sent to each index and the final target query.
/// </summary>
public class SearchTests
{
    private static string Values(string property, string kind, params string[] values) =>
        $$"""{ "nodes": [ { "kind": "{{kind}}", "property": "{{property}}", "values": [{{string.Join(",", values)}}] } ] }""";

    private static string Text(string property, params string[] values) => Values(property, "value-string", values.Select(value => $"\"{value}\"").ToArray());


    [Fact]
    public async Task Without_criteria_only_the_target_is_searched()
    {
        var indices = new FakeIndices();

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.All);

        Assert.Single(indices.Images.Requests);
        Assert.Empty(indices.Specimens.Requests);
    }

    [Fact]
    public async Task Text_values_match_the_keyword_sub_field()
    {
        var indices = new FakeIndices();

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""{ "images": { "include": {{Text("type", "MR")}} } }"""), SearchScope.All);

        Query.Requires(indices.Images.Requests.Single(), "type.keyword", "MR");
    }

    [Fact]
    public async Task Keys_of_included_documents_are_required_and_intersected()
    {
        var indices = new FakeIndices();
        indices.Specimens.Returns("specimens_keys", "1", "2", "3");
        indices.Sms.Returns("specimens_keys", "2", "3", "4");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""
            { "specimens": { "include": {{Text("type", "Material")}} },
              "sms": { "include": {{Text("type", "SNV")}} } }
            """), SearchScope.All);

        // The SM stage is narrowed by the specimens found before, the target requires the intersection.
        Query.Requires(indices.Sms.Requests.Single(), "specimens.id", "1", "2", "3");
        Query.Requires(indices.Images.Requests.Single(), "specimens.id", "2", "3");
    }

    [Fact]
    public async Task Search_ends_early_when_included_documents_are_not_found()
    {
        var indices = new FakeIndices();
        indices.Sms.Returns("specimens_keys");

        var result = await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""{ "sms": { "include": {{Text("type", "SNV")}} } }"""), SearchScope.All);

        Assert.Equal(0, result.Total);
        Assert.Empty(indices.Images.Requests);
    }

    [Fact]
    public async Task Search_ends_early_when_included_keys_have_nothing_in_common()
    {
        var indices = new FakeIndices();
        indices.Specimens.Returns("specimens_keys", "1", "2");
        indices.Sms.Returns("specimens_keys", "5");

        var result = await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""
            { "specimens": { "include": {{Text("type", "Material")}} },
              "sms": { "include": {{Text("type", "SNV")}} } }
            """), SearchScope.All);

        Assert.Equal(0, result.Total);
        Assert.Empty(indices.Images.Requests);
    }

    [Fact]
    public async Task Keys_of_excluded_documents_are_united_and_excluded()
    {
        var indices = new FakeIndices();
        indices.Sms.Returns("specimens_keys", "2", "3");
        indices.Cnvs.Returns("specimens_keys", "3", "5");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""
            { "sms": { "exclude": {{Text("type", "SNV")}} },
              "cnvs": { "exclude": {{Text("type", "Loss")}} } }
            """), SearchScope.All);

        // CNVs run before SMs (same rank, by name): the SM stage skips the specimens the CNVs excluded.
        Query.Excludes(indices.Sms.Requests.Single(), "specimens.id", "3", "5");

        var images = indices.Images.Requests.Single();
        Query.Excludes(images, "specimens.id", "2", "3", "5");
        Assert.Empty(Query.Terms(images, "specimens.id").Required);
    }

    [Fact]
    public async Task Search_does_not_end_when_excluded_documents_are_not_found()
    {
        var indices = new FakeIndices();
        indices.Sms.Returns("specimens_keys");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""{ "sms": { "exclude": {{Text("type", "SNV")}} } }"""), SearchScope.All);

        Query.HasNoTerms(indices.Images.Requests.Single(), "specimens.id");
    }

    [Fact]
    public async Task Exclude_criteria_of_a_document_are_searched_within_its_included_keys()
    {
        var indices = new FakeIndices();
        indices.Sms.ReturnsNext(("specimens_keys", ["2", "3"])).ReturnsNext(("specimens_keys", ["3"]));

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""
            { "sms": { "include": {{Text("affectedFeatures.gene.symbol", "TP53")}},
                       "exclude": {{Text("affectedFeatures.gene.symbol", "IDH1")}} } }
            """), SearchScope.All);

        Query.Requires(indices.Sms.Requests[0], "affectedFeatures.gene.symbol.keyword", "TP53");
        Query.Requires(indices.Sms.Requests[1], "affectedFeatures.gene.symbol.keyword", "IDH1");
        Query.Requires(indices.Sms.Requests[1], "specimens.id", "2", "3");

        var images = indices.Images.Requests.Single();
        Query.Requires(images, "specimens.id", "2", "3");
        Query.Excludes(images, "specimens.id", "3");
    }

    [Fact]
    public async Task Negated_criteria_select_documents_not_matching_them()
    {
        var indices = new FakeIndices();
        indices.Sms.Returns("specimens_keys", "2");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("""
            { "sms": { "include": { "negate": true, "nodes": [ { "kind": "value-string", "property": "type", "values": ["SNV"] } ] } } }
            """), SearchScope.All);

        Query.Excludes(indices.Sms.Requests.Single(), "type.keyword", "SNV");
        Query.Requires(indices.Images.Requests.Single(), "specimens.id", "2");
    }

    [Fact]
    public async Task Target_exclude_criteria_exclude_target_documents()
    {
        var indices = new FakeIndices();

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""
            { "images": { "include": {{Text("type", "MR")}}, "exclude": {{Text("referenceId", "IMG-1")}} } }
            """), SearchScope.All);

        var images = indices.Images.Requests.Single();
        Query.Requires(images, "type.keyword", "MR");
        Query.Excludes(images, "referenceId.keyword", "IMG-1");
    }

    [Fact]
    public async Task Gene_criteria_narrow_gene_expressions_to_the_selected_genes()
    {
        // Old engine: GeneExpressionFiltersCollection applies the gene criteria to gene expressions.
        var indices = new FakeIndices();
        indices.Genes.Returns("specimens_keys", "1", "2").Returns("genes_keys", "100");
        indices.GeneExpressions.Returns("specimens_keys", "2");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""
            { "gene_expressions": { "include": { "nodes": [ { "kind": "range-double", "property": "expression", "from": 10 } ] } },
              "genes": { "include": {{Text("symbol", "EGFR")}} } }
            """), SearchScope.All);

        var expressions = indices.GeneExpressions.Requests.Single();
        Query.Requires(expressions, "gene.id", "100");
        Query.Requires(expressions, "specimen.id", "1", "2");
        Query.Requires(indices.Images.Requests.Single(), "specimens.id", "2");
    }

    [Fact]
    public async Task Gene_criteria_narrow_variants_to_the_selected_genes()
    {
        // Old engine: SmFiltersCollection applies the gene criteria to SMs through their affected genes.
        var indices = new FakeIndices();
        indices.Genes.Returns("specimens_keys", "1").Returns("genes_keys", "100");
        indices.Sms.Returns("specimens_keys", "1");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""
            { "genes": { "include": {{Text("symbol", "EGFR")}} },
              "sms": { "include": {{Text("type", "SNV")}} } }
            """), SearchScope.All);

        Query.Requires(indices.Sms.Requests.Single(), "affectedFeatures.gene.id", "100");
    }

    [Fact]
    public async Task Variant_search_requires_the_genes_found()
    {
        // Old engine: SmsSearchService applies the found gene IDs through AffectedFeatures[].Gene.Id.
        var indices = new FakeIndices();
        indices.Genes.Returns("genes_keys", "100", "101");

        await indices.SearchService<SmIndex>().Search(FakeIndices.Criteria($$"""{ "genes": { "include": {{Text("symbol", "EGFR")}} } }"""), SearchScope.All);

        Query.Requires(indices.Sms.Requests.Single(), "affectedFeatures.gene.id", "100", "101");
    }

    [Fact]
    public async Task Gene_search_requires_the_genes_affected_by_variants()
    {
        // Old engine: GenesSearchService aggregates AffectedFeatures[].Gene.Id from SMs.
        var indices = new FakeIndices();
        indices.Sms.Returns("genes_keys", "100");

        await indices.SearchService<GeneIndex>().Search(FakeIndices.Criteria($$"""{ "sms": { "include": {{Text("type", "SNV")}} } }"""), SearchScope.All);

        Assert.EndsWith("AffectedFeatures.First().Gene.Id", indices.Sms.Collected.Single()["genes_keys"]);
        Query.Requires(indices.Genes.Requests.Single(), "id", "100");
    }

    [Fact]
    public async Task Project_search_bridges_specimens_up_to_donors()
    {
        // Old engine: ProjectsSearchService applies specimen IDs through Donors[].Specimens[].Id.
        var indices = new FakeIndices();
        indices.Sms.Returns("specimens_keys", "2", "3");
        indices.Specimens.Returns("donors_keys", "10");

        await indices.SearchService<ProjectIndex>().Search(FakeIndices.Criteria($$"""{ "sms": { "include": {{Text("type", "SNV")}} } }"""), SearchScope.All);

        Query.Requires(indices.Specimens.Requests.Single(), "id", "2", "3");
        Query.Requires(indices.Projects.Requests.Single(), "donors.id", "10");
    }

    [Fact]
    public async Task Gene_search_bridges_donors_down_to_specimens()
    {
        var indices = new FakeIndices();
        indices.Projects.Returns("donors_keys", "10", "11");
        indices.Specimens.Returns("specimens_keys", "1", "2", "5");

        await indices.SearchService<GeneIndex>().Search(FakeIndices.Criteria($$"""{ "projects": { "include": {{Text("name", "GBM")}} } }"""), SearchScope.All);

        Query.Requires(indices.Specimens.Requests.Single(), "donor.id", "10", "11");
        Query.Requires(indices.Genes.Requests.Single(), "specimens.id", "1", "2", "5");
    }

    [Fact]
    public async Task Excluded_keys_are_bridged_too()
    {
        var indices = new FakeIndices();
        indices.Sms.Returns("specimens_keys", "2");
        indices.Specimens.Returns("donors_keys", "10");

        await indices.SearchService<ProjectIndex>().Search(FakeIndices.Criteria($$"""{ "sms": { "exclude": {{Text("type", "SNV")}} } }"""), SearchScope.All);

        Query.Excludes(indices.Projects.Requests.Single(), "donors.id", "10");
    }
}
