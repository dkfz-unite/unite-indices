using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class InterventionFilters<T> : FiltersCollection<T> where T : class
{
    protected SpecimenFilterNames FilterNames = new();

    public InterventionFilters(SpecimenCriteria criteria, Expression<Func<T, InterventionIndex[]>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Intervention))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Intervention,
                path.Join(intervention => intervention.First().Type),
                criteria.Intervention
            ));
        }
    }
}
