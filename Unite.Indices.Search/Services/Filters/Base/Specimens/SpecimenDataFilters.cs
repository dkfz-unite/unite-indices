using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities;
using Unite.Indices.Entities.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class SpecimenDataFilters<T> : FiltersCollection<T> where T : class
{
    protected SpecimenFilterNames FilterNames = new();

    public SpecimenDataFilters(SpecimenCriteria criteria, Expression<Func<T, DataIndex>> path)
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

        if (IsNotEmpty(criteria.HasMeth))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasMeth,
                path.Join(data => data.Meth),
                criteria.HasMeth
            ));
        }
    }
}
