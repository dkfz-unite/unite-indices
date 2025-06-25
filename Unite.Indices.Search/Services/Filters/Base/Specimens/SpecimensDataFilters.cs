using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class SpecimensDataFilters<T> : FiltersCollection<T> where T : class
{
    protected SpecimensFilterNames FilterNames = new();

    public SpecimensDataFilters(SpecimensCriteria criteria, Expression<Func<T, DataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.HasExp))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasExp,
                path.Join(data => data.Exp),
                criteria.HasExp.Value
            ));
        }

        if (IsNotEmpty(criteria.HasExpSc))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasExpSc,
                path.Join(data => data.ExpSc),
                criteria.HasExpSc.Value
            ));
        }

        if (IsNotEmpty(criteria.HasSms))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasSms,
                path.Join(data => data.Sms),
                criteria.HasSms.Value
            ));
        }

        if (IsNotEmpty(criteria.HasCnvs))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasCnvs,
                path.Join(data => data.Cnvs),
                criteria.HasCnvs.Value
            ));
        }

        if (IsNotEmpty(criteria.HasSvs))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasSvs,
                path.Join(data => data.Svs),
                criteria.HasSvs.Value
            ));
        }

        if (IsNotEmpty(criteria.HasMeth))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasMeth,
                path.Join(data => data.Meth),
                criteria.HasMeth.Value
            ));
        }
    }
}
