using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Genes.Constants;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Genes;

public class GenesDataFilters<T> : FiltersCollection<T> where T : class
{
    protected GenesFilterNames FilterNames = new();

    public GenesDataFilters(GenesCriteria criteria, Expression<Func<T, DataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.HasSms))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasSms,
                path.Join(data => data.Sms),
                criteria.HasSms
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
    }
}
