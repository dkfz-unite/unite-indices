using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Donors;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Constants;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Donors;

public class DonorsFilters<T> : FiltersCollection<T> where T : class
{
    protected DonorsFilterNames FilterNames = new();

    public DonorsFilters(DonorsCriteria criteria, Expression<Func<T, DonorNavIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                FilterNames.Id,
                path.Join(donor => donor.Id),
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
    }
}
