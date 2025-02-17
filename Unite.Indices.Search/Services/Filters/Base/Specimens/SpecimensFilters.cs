using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class SpecimensFilters<T> : FiltersCollection<T> where T : class
{
    protected SpecimensFilterNames FilterNames = new();

    public SpecimensFilters(SpecimensCriteria criteria, Expression<Func<T, SpecimenNavIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                FilterNames.Id,
                path.Join(specimen => specimen.Id),
                criteria.Id
            ));
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.ReferenceId,
                path.Join(donor => donor.ReferenceId),
                criteria.ReferenceId
            ));
        }

        if (IsNotEmpty(criteria.SpecimenType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.SpecimenType,
                path.Join(specimen => specimen.Type.Suffix(_keywordSuffix)),
                criteria.SpecimenType
            ));
        }
    }
}
