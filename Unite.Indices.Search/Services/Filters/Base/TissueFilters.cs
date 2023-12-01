using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class TissueFilters<T> : SpecimenFilters<T> where T : class
{
    public TissueFilters(TissueCriteria criteria, Expression<Func<T, SpecimenIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                TissueFilterNames.ReferenceId,
                path.Join(specimen => specimen.Tissue.ReferenceId),
                criteria.ReferenceId)
            );
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                TissueFilterNames.Type,
                path.Join(specimen => specimen.Tissue.Type.Suffix(_keywordSuffix)),
                criteria.Type)
            );
        }

        if (IsNotEmpty(criteria.TumorType))
        {
            Add(new EqualityFilter<T, object>(
                TissueFilterNames.TumorType,
                path.Join(specimen => specimen.Tissue.TumorType.Suffix(_keywordSuffix)),
                criteria.TumorType)
            );
        }

        if (IsNotEmpty(criteria.Source))
        {
            Add(new SimilarityFilter<T, string>(
                TissueFilterNames.Source,
                path.Join(specimen => specimen.Tissue.Source),
                criteria.Source)
            );
        }
    }
}
