using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Projects;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Projects.Constants;
using Unite.Indices.Search.Services.Filters.Base.Projects.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Projects;

public class ProjectFilters<T> : FiltersCollection<T> where T : class
{
    protected ProjectFilterNames FilterNames = new();

    public ProjectFilters(ProjectCriteria criteria, Expression<Func<T, ProjectIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Name))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Name,
                path.Join(project => project.Name),
                criteria.Name.Value
            ));
        }
    }
}
