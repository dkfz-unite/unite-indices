using System.Linq.Expressions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class OrganoidFilters<T> : SpecimenFilters<T> where T : class
{
    public OrganoidFilters(OrganoidCriteria criteria, Expression<Func<T, SpecimenIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                OrganoidFilterNames.ReferenceId,
                path.Join(specimen => specimen.Organoid.ReferenceId),
                criteria.ReferenceId)
            );
        }

        if (IsNotEmpty(criteria.Medium))
        {
            Add(new SimilarityFilter<T, string>(
                OrganoidFilterNames.Medium,
                path.Join(specimen => specimen.Organoid.Medium),
                criteria.Medium)
            );
        }

        if (IsNotEmpty(criteria.Tumorigenicity))
        {
            Add(new BooleanFilter<T>(
                OrganoidFilterNames.Tumorigenicity,
                path.Join(specimen => specimen.Organoid.Tumorigenicity),
                criteria.Tumorigenicity)
            );
        }

        if (IsNotEmpty(criteria.Intervention))
        {
            Add(new SimilarityFilter<T, string>(
                OrganoidFilterNames.Intervention,
                path.Join(specimen => specimen.Organoid.Interventions.First().Type),
                criteria.Intervention)
            );
        }
    }
}
