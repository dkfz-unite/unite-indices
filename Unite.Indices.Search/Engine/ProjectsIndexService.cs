using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Projects;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine.Queries;

namespace Unite.Indices.Search.Engine;

public class ProjectsIndexService(IElasticOptions options) : IndexService<ProjectIndex>(options)
{
    protected override string Collection => IndexNames.Projects;
    
    public async Task<SearchResult<ProjectIndex>> GetAccessibleProjects(int? userId)
    {
        if(userId == null)
            return new SearchResult<ProjectIndex>();
    
        var query = new SearchQuery<ProjectIndex>()
            .AddPagination(0, 10_000)
            .AddFilters([
                new CompoundFilter<ProjectIndex>("UserFilter", false, 
                    new BooleanFilter<ProjectIndex>("IsPublic", x => x.IsPublic, true), 
                    new EqualityFilter<ProjectIndex, int>(
                        "UserId",
                        false,
                        x => x.Users.First().UserId,
                        [ userId.Value ]
                    ),
                    LogicalOperator.Or)
            ])
            .AddOrdering(project => project.Stats.Donors.Number);
    
        return await Search(query);
    }
}
