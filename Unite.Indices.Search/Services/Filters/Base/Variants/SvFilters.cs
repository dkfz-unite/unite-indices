using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Omics.Variants;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Constants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public class SvFilters<T> : VariantFilters<T, SvIndex> where T : class
{
    protected override SvFilterNames FilterNames => new();

    public SvFilters(SvCriteria criteria, Expression<Func<T, SvIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }
        
        if (IsNotEmpty(criteria.Position))
        {
            Remove(FilterNames.Position);

            Add(new MultiPropertyRangeFilter<T, int>(
                FilterNames.Position,
                path.Join(variant => variant.End),
                path.Join(variant => variant.OtherStart),
                criteria.Position.Value?.From,
                criteria.Position.Value?.To
            ));
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Type,
                path.Join(variant => variant.Type.Suffix(_keywordSuffix)),
                criteria.Type.Value
            ));
        }

        if (IsNotEmpty(criteria.Inverted))
        {
            Add(new BooleanFilter<T>(
                FilterNames.Inverted,
                path.Join(variant => variant.Inverted),
                criteria.Inverted.Value
            ));
        }
    }
}
