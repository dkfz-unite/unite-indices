using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Constants;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Donors;

public class DonorDataFilters<T> : FiltersCollection<T> where T : class
{
    protected DonorFilterNames FilterNames = new();

    public DonorDataFilters(DonorCriteria criteria, Expression<Func<T, DataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.HasSsms))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasSsms,
                path.Join(data => data.Ssms),
                criteria.HasSsms
            ));
        }

        if (IsNotEmpty(criteria.HasCnvs))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasCnvs,
                path.Join(data => data.Cnvs),
                criteria.HasCnvs
            ));
        }

        if (IsNotEmpty(criteria.HasSvs))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasSvs,
                path.Join(data => data.Svs),
                criteria.HasSvs
            ));
        }

        if (IsNotEmpty(criteria.HasGeneExp))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasGeneExp,
                path.Join(data => data.GeneExp),
                criteria.HasGeneExp
            ));
        }

        if (IsNotEmpty(criteria.HasGeneExpSc))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasGeneExpSc,
                path.Join(data => data.GeneExpSc),
                criteria.HasGeneExpSc
            ));
        }
    }
}
