using Unite.Indices.Search.Engine2.Access;
using Unite.Indices.Search.Tests.Support;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;

namespace Unite.Indices.Search.Tests;

/// <summary>
/// Access restriction: users see data of public projects and of projects they are members of (old engine: PersonalizeDonorsCriteria),
/// by project IDs, as the first stage on donors.
/// </summary>
public class AccessTests
{
    private const int UserId = 7;

    private static FakeIndices UserWithProjects(params string[] projects)
    {
        var indices = new FakeIndices();
        indices.Projects.Returns("projects_keys", projects);
        return indices;
    }

    private static string Text(string property, string value) =>
        $$"""{ "nodes": [ { "kind": "value-string", "property": "{{property}}", "values": ["{{value}}"] } ] }""";


    [Fact]
    public async Task Searches_with_all_data_are_not_restricted()
    {
        var indices = new FakeIndices();

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.All);

        Assert.Empty(indices.Projects.Requests);
        Assert.Empty(indices.Donors.Requests);
        Query.HasNoTerms(indices.Images.Requests.Single(), "donor.id");
    }

    [Fact]
    public async Task Accessible_projects_are_public_projects_and_projects_of_the_user()
    {
        var indices = UserWithProjects("1", "2");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(UserId));

        var projects = indices.Projects.Requests.Single();
        Assert.Contains("\"isPublic\":{\"value\":true}", projects);
        Query.Requires(projects, "users.userId", UserId.ToString());
    }

    [Fact]
    public async Task Access_restricts_donors_by_project_ids_and_the_target_by_their_donors()
    {
        var indices = UserWithProjects("1", "2");
        indices.Donors.Returns("donors_keys", "10", "11");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(UserId));

        Query.Requires(indices.Donors.Requests.Single(), "projects.id", "1", "2");
        Query.Requires(indices.Images.Requests.Single(), "donor.id", "10", "11");
    }

    [Fact]
    public async Task Access_runs_first_and_narrows_the_other_stages()
    {
        var indices = UserWithProjects("1");
        indices.Donors.Returns("donors_keys", "10");
        indices.Specimens.Returns("specimens_keys", "5");

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria($$"""{ "specimens": { "include": {{Text("type", "Material")}} } }"""),
            SearchScope.ForUser(UserId));

        Query.Requires(indices.Specimens.Requests.Single(), "donor.id", "10");

        var images = indices.Images.Requests.Single();
        Query.Requires(images, "donor.id", "10");
        Query.Requires(images, "specimens.id", "5");
    }

    [Fact]
    public async Task Access_is_applied_through_specimens_for_targets_without_donors()
    {
        var indices = UserWithProjects("1");
        indices.Donors.Returns("specimens_keys", "5", "6").Returns("donors_keys", "10");

        await indices.SearchService<GeneIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(UserId));

        Query.Requires(indices.Genes.Requests.Single(), "specimens.id", "5", "6");

        // The donor stage delivered specimen keys for the target: its donor keys are not bridged.
        Assert.Empty(indices.Specimens.Requests);
    }

    [Fact]
    public async Task Access_on_the_target_itself_goes_into_its_query()
    {
        var indices = UserWithProjects("1", "2");

        await indices.SearchService<DonorIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(UserId));

        Query.Requires(indices.Donors.Requests.Single(), "projects.id", "1", "2");
    }

    [Fact]
    public async Task Users_without_accessible_projects_find_nothing()
    {
        var indices = UserWithProjects();

        var result = await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(UserId));

        Assert.Equal(0, result.Total);
        Assert.Empty(indices.Donors.Requests);
        Assert.Empty(indices.Images.Requests);
    }

    [Fact]
    public async Task Anonymous_users_find_nothing_without_any_request()
    {
        var indices = UserWithProjects("1");

        var result = await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(null));

        Assert.Equal(0, result.Total);
        Assert.Empty(indices.Projects.Requests);
        Assert.Empty(indices.Images.Requests);
    }

    [Fact]
    public async Task The_list_of_projects_is_public_and_marked_by_access()
    {
        var indices = UserWithProjects("1");
        indices.Projects.ReturnsRows([new ProjectIndex { Id = 1 }, new ProjectIndex { Id = 2 }]);

        var result = await indices.SearchService<ProjectIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(UserId));

        Assert.Empty(indices.Donors.Requests);
        Assert.Equal([true, false], result.Rows.Select(project => project.IsAccessible).ToArray());
    }

    [Fact]
    public async Task Projects_found_by_criteria_on_other_documents_use_accessible_data_only()
    {
        var indices = UserWithProjects("1");
        indices.Donors.Returns("donors_keys", "10", "11");
        indices.Sms.Returns("specimens_keys", "5");
        indices.Specimens.Returns("donors_keys", "11", "12");

        await indices.SearchService<ProjectIndex>().Search(FakeIndices.Criteria($$"""{ "sms": { "include": {{Text("type", "SNV")}} } }"""),
            SearchScope.ForUser(UserId));

        // Donors of SM specimens (11, 12) intersected with accessible donors (10, 11).
        var projects = indices.Projects.Requests.Last();
        Query.Requires(projects, "donors.id", "11");

        // Accessible projects are looked up once for the restriction and the IsAccessible annotation.
        Assert.Equal(2, indices.Projects.Requests.Count);
    }

    [Fact]
    public async Task Searches_of_users_need_an_access_service()
    {
        var indices = new FakeIndices(withAccessService: false);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.ForUser(UserId)));

        await indices.SearchService<ImageIndex>().Search(FakeIndices.Criteria("{}"), SearchScope.All);
    }

    [Fact]
    public async Task Get_returns_documents_in_the_scope_only()
    {
        var indices = UserWithProjects("1");
        indices.Donors.Returns("donors_keys", "10");
        indices.Images.ReturnsRows([new ImageIndex { Id = 3 }], []);

        var found = await indices.SearchService<ImageIndex>().Get("3", SearchScope.ForUser(UserId));
        var missing = await indices.SearchService<ImageIndex>().Get("4", SearchScope.ForUser(UserId));

        Assert.Equal(3, found.Id);
        Assert.Null(missing);
        Query.Requires(indices.Images.Requests[0], "id", "3");
        Query.Requires(indices.Images.Requests[0], "donor.id", "10");
    }

    [Fact]
    public async Task Get_with_an_invalid_id_finds_nothing_without_any_request()
    {
        var indices = new FakeIndices();

        Assert.Null(await indices.SearchService<ImageIndex>().Get("abc", SearchScope.All));
        Assert.Empty(indices.Images.Requests);
    }

    [Fact]
    public async Task Stats_run_the_stages_once_and_page_through_the_target()
    {
        var indices = UserWithProjects("1");
        indices.Donors.Returns("donors_keys", "10");
        var size = Engine2.SearchService<ImageIndex>.StatsPageSize;
        indices.Images.ReturnsRows(
            Enumerable.Range(1, size).Select(id => new ImageIndex { Id = id, Data = new() }).ToArray(),
            [new ImageIndex { Id = size + 1, Data = new() }]);

        var stats = await indices.SearchService<ImageIndex>().Stats(FakeIndices.Criteria("{}"), SearchScope.ForUser(UserId));

        Assert.Equal(size + 1, stats.Count);
        Assert.Single(indices.Donors.Requests);
        Assert.Equal(2, indices.Images.Requests.Count);
        Assert.All(indices.Images.Requests, request => Query.Requires(request, "donor.id", "10"));
    }
}
