using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Omics.Variants;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Constants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public class CnvFilters<T> : VariantFilters<T, CnvIndex> where T : class
{
    protected override CnvFilterNames FilterNames => new();

    public CnvFilters(CnvCriteria criteria, Expression<Func<T, CnvIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }
        
        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Type,
                path.Join(variant => variant.Type.Suffix(_keywordSuffix)),
                criteria.Type.Value
            ));
        }

        if (IsNotEmpty(criteria.Loh))
        {
            Add(new BooleanFilter<T>(
                FilterNames.Loh,
                path.Join(variant => variant.Loh),
                criteria.Loh.Value
            ));
        }

        if (IsNotEmpty(criteria.Del))
        {
            Add(new BooleanFilter<T>(
                FilterNames.Del,
                path.Join(variant => variant.Del),
                criteria.Del.Value
            ));
        }
    }
}
