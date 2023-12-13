using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Genome.Variants;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Constants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public class CnvFilters<T> : VariantBaseFilters<T, CnvIndex> where T : class
{
    protected override CnvFilterNames FilterNames => new();

    public CnvFilters(CnvCriteria criteria, Expression<Func<T, CnvIndex>> path) : base(criteria, path)
    {
        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Type,
                path.Join(variant => variant.Type.Suffix(_keywordSuffix)),
                criteria.Type
            ));
        }

        if (IsNotEmpty(criteria.Loh))
        {
            Add(new BooleanFilter<T>(
                FilterNames.Loh,
                path.Join(variant => variant.Loh),
                criteria.Loh
            ));
        }

        if (IsNotEmpty(criteria.HomoDel))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HomoDel,
                path.Join(variant => variant.HomoDel),
                criteria.HomoDel
            ));
        }
    }
}
