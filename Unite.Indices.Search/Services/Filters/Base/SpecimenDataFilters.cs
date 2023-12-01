using System.Linq.Expressions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class SpecimenDataFilters<T> : FiltersCollection<T> where T : class
{
    public SpecimenDataFilters(SpecimenCriteriaBase criteria, Expression<Func<T, DataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.HasDrugs))
        {
            Add(new BooleanFilter<T>(
                SpecimenFilterNames.HasDrugs,
                path.Join(data => data.Drugs),
                criteria.HasDrugs)
            );
        }

        if (IsNotEmpty(criteria.HasSsms))
        {
            Add(new BooleanFilter<T>(
                SpecimenFilterNames.HasSsms,
                path.Join(data => data.Ssms),
                criteria.HasSsms)
            );
        }

        if (IsNotEmpty(criteria.HasCnvs))
        {
            Add(new BooleanFilter<T>(
                SpecimenFilterNames.HasCnvs,
                path.Join(data => data.Cnvs),
                criteria.HasCnvs)
            );
        }

        if (IsNotEmpty(criteria.HasSvs))
        {
            Add(new BooleanFilter<T>(
                SpecimenFilterNames.HasSvs,
                path.Join(data => data.Svs),
                criteria.HasSvs)
            );
        }

        if (IsNotEmpty(criteria.HasGeneExp))
        {
            Add(new BooleanFilter<T>(
                SpecimenFilterNames.HasGeneExp,
                path.Join(data => data.GeneExp),
                criteria.HasGeneExp)
            );
        }
    }
}
