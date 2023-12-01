using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class VariantFilters<T> : FiltersCollection<T> where T : class
{
    public VariantFilters(VariantCriteriaBase criteria, Expression<Func<T, VariantIndex>> path)
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
                criteria.Id)
            );
        }
    }
}
