using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Projects;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Projects.Constants;
using Unite.Indices.Search.Services.Filters.Base.Projects.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Projects;

public class ProjectsFilters<T> : FiltersCollection<T> where T : class
{
    protected ProjectsFilterNames FilterNames = new();

    public ProjectsFilters(ProjectsCriteria criteria, Expression<Func<T, ProjectNavIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                FilterNames.Id,
                path.Join(project => project.Id),
                criteria.Id.Value
            ));
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
