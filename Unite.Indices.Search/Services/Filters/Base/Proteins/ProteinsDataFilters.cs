using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Constants;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Proteins;

public class ProteinsDataFilters<T> : FiltersCollection<T> where T : class
{
    protected ProteinsFilterNames FilterNames = new();

    public ProteinsDataFilters(ProteinsCriteria criteria, Expression<Func<T, DataIndex>> path)
    {
        if (criteria == null)
        {
            return;
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

        if (IsNotEmpty(criteria.HasCnvps))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasCnvps,
                criteria.HasCnvps.Not,
                path.Join(data => data.Cnvps),
                criteria.HasCnvps.Value
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
    }
}
