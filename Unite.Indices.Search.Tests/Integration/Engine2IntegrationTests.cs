using Unite.Indices.Search.Engine2;
using Unite.Indices.Search.Engine2.Access;
using Unite.Indices.Search.Tests.Support;
using static Unite.Indices.Search.Tests.Integration.Searches;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;

namespace Unite.Indices.Search.Tests.Integration;

/// <summary>
/// Engine2 against a real Elasticsearch with the fixture dataset (see <see cref="Dataset"/>):
/// field mappings, cross references, access, sorting, Get, Stats, free text and paged key collection.
/// </summary>
[Collection(nameof(ElasticCollection))]
public class Engine2IntegrationTests(ElasticFixture elastic)
{
    private async Task<Found> Find(string document, string criteria, SearchScope scope = null, string term = null)
    {
        Skip.If(elastic.Unavailable != null, elastic.Unavailable);

        return await Searches.Search(elastic.Services, document, criteria, scope ?? SearchScope.All, term);
    }

    private async Task<string[]> Ids(string document, string criteria, SearchScope scope = null)
    {
        var found = await Find(document, criteria, scope);

        Assert.Equal(found.Total, found.Ids.Length);
        return [.. found.Ids.Order()];
    }

    private SearchService<T> Service<T>() where T : class
    {
        Skip.If(elastic.Unavailable != null, elastic.Unavailable);

        return new SearchService<T>(elastic.Services);
    }


    [SkippableTheory]
    [InlineData("projects", 3)]
    [InlineData("donors", 4)]
    [InlineData("specimens", 5)]
    [InlineData("images", 3)]
    [InlineData("genes", 3)]
    [InlineData("gene_expressions", 3)]
    [InlineData("proteins", 1)]
    [InlineData("protein_expressions", 1)]
    [InlineData("sms", 3)]
    [InlineData("cnvs", 1)]
    [InlineData("svs", 1)]
    [InlineData("cnv-profiles", 1)]
    public async Task Every_document_is_searchable_with_its_sort_orders(string document, int count)
    {
        var found = await Find(document, "{}");

        Assert.Equal(count, found.Total);
    }

    [SkippableFact]
    public async Task Text_values_match_the_keyword_fields()
    {
        Assert.Equal(["1", "3"], await Ids("donors", Criteria(Document("donors", [Text("clinicalData.sex", "Male")]))));

        // Exact values only: a text match would also find "Female".
        Assert.Equal(["2", "4"], await Ids("donors", Criteria(Document("donors", [Text("clinicalData.sex", "Female")]))));
    }

    [SkippableFact]
    public async Task Ranges_and_booleans_match_nested_fields()
    {
        Assert.Equal(["2", "3"], await Ids("images", Criteria(Document("images", [Range("double", "mr.wholeTumor", 20, 60)]))));
        Assert.Equal(["1", "4", "5"], await Ids("specimens", Criteria(Document("specimens", [Bool("material.molecularData.idhStatus", true)]))));
        Assert.Equal(["3"], await Ids("donors", Criteria(Document("donors", [Range("int", "clinicalData.age", 50, 59)]))));
    }

    [SkippableFact]
    public async Task Mutations_and_specimen_criteria_must_match_the_same_specimen()
    {
        // D1 has an SNV in S1 (Material) and a Line S2 without mutations.
        var line = await Ids("donors", Criteria(
            Document("sms", [Text("type", "SNV")]),
            Document("specimens", [Text("type", "Line")])));

        var material = await Ids("donors", Criteria(
            Document("sms", [Text("type", "SNV")]),
            Document("specimens", [Text("type", "Material")])));

        Assert.Empty(line);
        Assert.Equal(["1", "3"], material);
    }

    [SkippableFact]
    public async Task Gene_criteria_find_variants_through_affected_features_and_samples_through_specimens()
    {
        var egfr = Criteria(Document("genes", [Text("symbol", "EGFR")]));

        Assert.Equal(["1"], await Ids("sms", egfr));
        Assert.Equal(["1"], await Ids("cnvs", egfr));
        Assert.Empty(await Ids("svs", egfr));
        Assert.Equal(["1", "2"], await Ids("images", egfr));
        Assert.Equal(["1"], await Ids("proteins", egfr));
    }

    [SkippableFact]
    public async Task Protein_criteria_find_variants_through_transcripts()
    {
        Assert.Equal(["1"], await Ids("sms", Criteria(Document("proteins", [Text("symbol", "EGFR")]))));
    }

    [SkippableFact]
    public async Task Expression_criteria_find_samples_by_specimen()
    {
        Assert.Equal(["1"], await Ids("images", Criteria(Document("gene_expressions", [Range("double", "expression", 1)]))));
        Assert.Equal(["1"], await Ids("donors", Criteria(Document("protein_expressions", [Range("double", "expression", 1)]))));
    }

    [SkippableFact]
    public async Task Excluded_documents_remove_their_specimens()
    {
        Assert.Equal(["2"], await Ids("images", Criteria(Document("sms", exclude: [Text("type", "SNV")]))));
        Assert.Equal(["2", "4"], await Ids("donors", Criteria(Document("donors", exclude: [Text("clinicalData.sex", "Male")]), Document("specimens", [Text("type", "Material")]))));
    }

    [SkippableFact]
    public async Task Projects_are_found_through_donors_of_specimens()
    {
        Assert.Equal(["1", "3"], await Ids("projects", Criteria(Document("sms", [Text("type", "DEL")]))));
        Assert.Equal(["1", "2"], await Ids("projects", Criteria(Document("sms", [Text("type", "SNV")]))));
    }

    [SkippableFact]
    public async Task Project_criteria_restrict_documents_below_donors()
    {
        Assert.Equal(["1", "3"], await Ids("sms", Criteria(Document("projects", [Text("name", "Public")]))));
        Assert.Equal(["1", "2", "3"], await Ids("specimens", Criteria(Document("projects", [Text("name", "Public")]))));
    }

    [SkippableFact]
    public async Task Users_see_public_projects_and_projects_they_are_members_of()
    {
        // Users without memberships see public projects; anonymous users see nothing (as in the old engine).
        Assert.Equal(["1", "2", "3", "4"], await Ids("donors", "{}"));
        Assert.Equal(["1", "2", "3"], await Ids("donors", "{}", SearchScope.ForUser(Dataset.Member)));
        Assert.Equal(["1", "2", "4"], await Ids("donors", "{}", SearchScope.ForUser(Dataset.Stranger)));
        Assert.Equal(["1", "2"], await Ids("donors", "{}", SearchScope.ForUser(12345)));
        Assert.Empty(await Ids("donors", "{}", SearchScope.ForUser(null)));

        // Through specimens: S4 (D3) is not accessible for the stranger.
        Assert.Equal(["1", "3"], await Ids("sms", "{}", SearchScope.ForUser(Dataset.Stranger)));
        Assert.Equal(["1", "2", "3", "5"], await Ids("specimens", "{}", SearchScope.ForUser(Dataset.Stranger)));
        Assert.Equal(["1", "2"], await Ids("images", "{}", SearchScope.ForUser(Dataset.Stranger)));
    }

    [SkippableFact]
    public async Task The_list_of_projects_is_public_and_marked_by_access()
    {
        var result = await Service<ProjectIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(Dataset.Member));

        Assert.Equal(
            [("1", true), ("2", true), ("3", false)],
            result.Rows.Select(project => (project.Id.ToString(), project.IsAccessible)).OrderBy(project => project.Item1).ToArray());
    }

    [SkippableFact]
    public async Task Projects_found_by_criteria_on_other_documents_use_accessible_data_only()
    {
        // SM3 (DEL) is in S1 (P1) and S5 (P3); P3 is not accessible for the member.
        Assert.Equal(["1"], await Ids("projects", Criteria(Document("sms", [Text("type", "DEL")])), SearchScope.ForUser(Dataset.Member)));
    }

    [SkippableFact]
    public async Task Results_are_sorted_as_in_the_old_services()
    {
        // SM3 has two donors; SM1 (chr 7) and SM2 (chr 17) one each, ordered by chromosome number, not text.
        Assert.Equal(["3", "1", "2"], (await Find("sms", "{}")).Ids);

        // D1 has three genes.
        Assert.Equal("1", (await Find("donors", "{}")).Ids[0]);

        // P1 has two donors.
        Assert.Equal("1", (await Find("projects", "{}")).Ids[0]);
    }

    [SkippableFact]
    public async Task Free_text_searches_text_fields_of_the_target()
    {
        Assert.Equal(["1"], (await Find("genes", "{}", term: "EGFR")).Ids);
        Assert.Equal(["2", "4"], (await Find("donors", "{}", term: "Female")).Ids.Order().ToArray());
    }

    [SkippableFact]
    public async Task Get_returns_documents_in_the_scope_without_large_properties()
    {
        var specimens = Service<SpecimenIndex>();

        Assert.Equal(5, (await specimens.Get("5", SearchScope.ForUser(Dataset.Stranger)))?.Id);
        Assert.Null(await specimens.Get("5", SearchScope.ForUser(Dataset.Member)));
        Assert.Null(await specimens.Get("42", SearchScope.All));

        var sm = await Service<SmIndex>().Get("3", SearchScope.All);
        Assert.Equal("DEL", sm.Type);
        Assert.Null(sm.Specimens);

        var donor = await Service<DonorIndex>().Get("1", SearchScope.All);
        Assert.Equal(2, donor.Specimens.Length);
    }

    [SkippableFact]
    public async Task Stats_merge_the_data_of_every_document_found()
    {
        var stats = await Service<ImageIndex>().Stats(FakeIndices.Criteria("{}"), SearchScope.ForUser(Dataset.Stranger));

        Assert.Equal([1, 2], stats.Keys.Cast<int>().Order().ToArray());
        Assert.All(stats.Values, data => Assert.True(data.Mrs));
    }

    [SkippableFact]
    public async Task Keys_beyond_one_page_are_all_collected()
    {
        // All five specimens come from the gene stage: three pages of two keys.
        Assert.Equal(["1", "2", "3", "4", "5"], await Ids("specimens", Criteria(Document("genes", [Exists("symbol")]))));
        Assert.True(ElasticFixture.KeysPageSize < 5);
    }

    [SkippableFact]
    public async Task Excluded_keys_beyond_one_page_are_all_excluded()
    {
        // Specimens of all genes (five, over three pages) excluded: nothing is left.
        Assert.Empty(await Ids("specimens", Criteria(Document("genes", exclude: [Exists("symbol")]))));
    }
}
