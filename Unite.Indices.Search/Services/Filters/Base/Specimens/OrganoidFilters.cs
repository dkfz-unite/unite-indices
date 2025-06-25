using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class OrganoidFilters<T> : SpecimenFilters<T, OrganoidIndex> where T : class
{
    protected override OrganoidFilterNames FilterNames => new();
    

    public OrganoidFilters(OrganoidCriteria criteria, Expression<Func<T, OrganoidIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }
        
        if (IsNotEmpty(criteria.Medium))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Medium,
                path.Join(specimen => specimen.Medium),
                criteria.Medium.Value
            ));
        }

        if (IsNotEmpty(criteria.Tumorigenicity))
        {
            Add(new BooleanFilter<T>(
                FilterNames.Tumorigenicity,
                path.Join(specimen => specimen.Tumorigenicity),
                criteria.Tumorigenicity.Value
            ));
        }
    }
}
