using System.Linq.Expressions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class DrugScreeningFilters<T> : FiltersCollection<T> where T : class
{
    public DrugScreeningFilters(SpecimenCriteriaBase criteria, Expression<Func<T, DrugScreeningIndex[]>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Drug))
        {
            Add(new SimilarityFilter<T, string>(
                SpecimenFilterNames.Drug,
                path.Join(screenings => screenings.First().Drug),
                criteria.Drug)
            );
        }

        if (IsNotEmpty(criteria.Dss))
        {
            Add(new RangeFilter<T, double?>(
                SpecimenFilterNames.Dss,
                path.Join(screenings => screenings.First().Dss),
                criteria.Dss.From,
                criteria.Dss.To)
            );
        }

        if (IsNotEmpty(criteria.DssSelective))
        {
            Add(new RangeFilter<T, double?>(
                SpecimenFilterNames.DssSelective,
                path.Join(screenings => screenings.First().DssSelective),
                criteria.DssSelective.From,
                criteria.DssSelective.To)
            );
        }
    }
}
