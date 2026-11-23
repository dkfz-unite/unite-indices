using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Criteria.Properties;
using Unite.Indices.Search.Engine2.Fields;
using Unite.Indices.Search.Engine2.References;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;

namespace Unite.Indices.Search.Engine2.Access;

/// <summary>
/// Access by projects: users have access to public projects and to projects they are members of,
/// and through them to the donors of these projects and all data of the donors.
/// The list of projects itself is public: a search of projects without criteria on other documents is not restricted,
/// found projects are marked as accessible or not.
/// A scope is a snapshot: its accessible projects are looked up once, so create a scope per request.
/// </summary>
public class ProjectsAccessService(IServiceProvider serviceProvider) : IAccessService
{
    private const string ProjectKeys = "projects_keys";

    // Accessible projects are looked up once per scope (one search request), e.g. for the restriction and the annotation.
    private static readonly ConditionalWeakTable<SearchScope, Task<HashSet<int>>> _accessibleProjects = new();

    // Not a searchable field: clients can't filter by project membership, only the access service does.
    private static readonly SearchField<DonorIndex, int> _donorProjects = new("projects.id", donor => donor.Projects.First().Id);


    public async Task<AccessRestriction> GetRestriction(SearchScope scope, Type targetType, bool hasDocumentCriteria)
    {
        if (scope.IsAll)
            return null;

        if (targetType == typeof(ProjectIndex) && !hasDocumentCriteria)
            return null;

        var projects = await GetAccessibleProjects(scope);

        if (projects.Count == 0)
            return AccessRestriction.Nothing;

        var criteria = new CriteriaGroup
        {
            Nodes = [SearchLimits.CreateValueCriteria(_donorProjects, projects)]
        };

        return new AccessRestriction(typeof(DonorIndex), criteria);
    }

    public async Task Annotate<T>(IEnumerable<T> documents, SearchScope scope) where T : class
    {
        if (documents is not IEnumerable<ProjectIndex> projects)
            return;

        var accessible = scope.IsAll ? null : await GetAccessibleProjects(scope);

        foreach (var project in projects)
            project.IsAccessible = accessible == null || accessible.Contains(project.Id);
    }


    /// <summary>
    /// Public projects and projects the user is a member of; none without a user.
    /// </summary>
    private Task<HashSet<int>> GetAccessibleProjects(SearchScope scope)
    {
        return _accessibleProjects.GetValue(scope, FindAccessibleProjects);
    }

    private async Task<HashSet<int>> FindAccessibleProjects(SearchScope scope)
    {
        if (scope.UserId == null)
            return [];

        var query = new SearchQuery<ProjectIndex>().AddFilter(new CompoundFilter<ProjectIndex>("Access", false,
            [
                new BooleanFilter<ProjectIndex>("IsPublic", project => project.IsPublic, true),
                new EqualityFilter<ProjectIndex, int>("UserId", false, project => project.Users.First().UserId, [scope.UserId.Value])
            ],
            Engine.Filters.LogicalOperator.Or));

        var properties = new Dictionary<string, LambdaExpression> { [ProjectKeys] = (Expression<Func<ProjectIndex, int>>)(project => project.Id) };

        var keys = await serviceProvider.GetRequiredService<IIndexService<ProjectIndex>>().CollectKeys(query, properties);

        return keys[ProjectKeys].Select(int.Parse).ToHashSet();
    }
}
