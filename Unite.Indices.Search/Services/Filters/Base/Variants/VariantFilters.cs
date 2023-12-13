using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Genome.Variants;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Constants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public class VariantFilters<T> : FiltersCollection<T> where T : class
{
    public VariantFilters(VariantCriteria criteria, Expression<Func<T, VariantIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, object>(
                VariantFilterNames.Id,
                path.Join(variant => variant.Id.Suffix(_keywordSuffix)),
                criteria.Id
            ));
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                VariantFilterNames.Type,
                path.Join(variant => variant.Type.Suffix(_keywordSuffix)),
                criteria.Type
            ));
        }
    }
}
