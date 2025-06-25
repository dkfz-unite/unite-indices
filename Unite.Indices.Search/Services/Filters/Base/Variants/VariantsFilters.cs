using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Omics.Variants;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Constants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public class VariantsFilters<T> : FiltersCollection<T> 
    where T : class
{
    protected VariantsFilterNames FilterNames = new();

    public VariantsFilters(VariantsCriteria criteria, Expression<Func<T, VariantNavIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                FilterNames.Id,
                path.Join(variant => variant.Id),
                criteria.Id.Value
            ));
        }
    }
}
