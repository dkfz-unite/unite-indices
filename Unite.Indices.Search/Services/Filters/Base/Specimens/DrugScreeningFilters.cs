using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens.Drugs;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class DrugScreeningFilters<T> : FiltersCollection<T> where T : class
{
    protected SpecimenFilterNames FilterNames = new();

    public DrugScreeningFilters(SpecimenBaseCriteria criteria, Expression<Func<T, DrugScreeningIndex[]>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Drug))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Drug,
                path.Join(screenings => screenings.First().Drug),
                criteria.Drug
            ));
        }

        if (IsNotEmpty(criteria.Dss))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.Dss,
                path.Join(screenings => screenings.First().Dss),
                criteria.Dss.From,
                criteria.Dss.To
            ));
        }

        if (IsNotEmpty(criteria.DssSelective))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.DssSelective,
                path.Join(screenings => screenings.First().DssSelective),
                criteria.DssSelective.From,
                criteria.DssSelective.To
            ));
        }
    }
}
