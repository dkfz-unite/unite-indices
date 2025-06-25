using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Omics.Variants;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Constants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public class SmFilters<T> : VariantFilters<T, SmIndex> where T : class
{
    protected override SmFilterNames FilterNames => new();

    public SmFilters(SmCriteria criteria, Expression<Func<T, SmIndex>> path) : base(criteria, path)
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
    }
}
