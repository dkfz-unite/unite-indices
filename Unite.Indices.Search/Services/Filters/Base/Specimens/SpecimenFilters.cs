using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class SpecimenFilters<T> : FiltersCollection<T> where T : class
{
    public SpecimenFilters(SpecimenCriteria criteria, Expression<Func<T, SpecimenIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                SpecimenFilterNames.SpecimenId,
                path.Join(specimen => specimen.Id),
                criteria.Id
            ));
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                SpecimenFilterNames.SpecimenType,
                path.Join(specimen => specimen.Type.Suffix(_keywordSuffix)),
                criteria.Type
            ));
        }
    }
}
