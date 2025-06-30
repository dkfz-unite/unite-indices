using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Constants;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Donors;

public class DonorsDataFilters<T> : FiltersCollection<T> where T : class
{
    protected DonorsFilterNames FilterNames = new();

    public DonorsDataFilters(DonorsCriteria criteria, Expression<Func<T, DataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.HasExp))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasExp,
                criteria.HasExp.Not,
                path.Join(data => data.Exp),
                criteria.HasExp.Value
            ));
        }

        if (IsNotEmpty(criteria.HasExpSc))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasExpSc,
                criteria.HasExpSc.Not,
                path.Join(data => data.ExpSc),
                criteria.HasExpSc.Value
            ));
        }

        if (IsNotEmpty(criteria.HasSms))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasSms,
                criteria.HasSms.Not,
                path.Join(data => data.Sms),
                criteria.HasSms.Value
            ));
        }

        if (IsNotEmpty(criteria.HasCnvs))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasCnvs,
                criteria.HasCnvs.Not,
                path.Join(data => data.Cnvs),
                criteria.HasCnvs.Value
            ));
        }

        if (IsNotEmpty(criteria.HasSvs))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasSvs,
                criteria.HasSvs.Not,
                path.Join(data => data.Svs),
                criteria.HasSvs.Value
            ));
        }

        if (IsNotEmpty(criteria.HasMeth))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasMeth,
                criteria.HasMeth.Not,
                path.Join(data => data.Meth),
                criteria.HasMeth.Value
            ));
        }
    }
}
