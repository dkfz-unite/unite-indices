using System.Linq.Expressions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Essentials.Extensions;
using Nest;

namespace Unite.Indices.Search.Services.Filters.Base;

public class CellLineFilters<T> : SpecimenFilters<T> where T : class
{
    public CellLineFilters(CellLineCriteria criteria, Expression<Func<T, SpecimenIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                CellLineFilterNames.ReferenceId,
                path.Join(specimen => specimen.Cell.ReferenceId),
                criteria.ReferenceId)
            );
        }

        if (IsNotEmpty(criteria.Species))
        {
            Add(new EqualityFilter<T, object>(
                CellLineFilterNames.Species,
                path.Join(specimen => specimen.Cell.Species.Suffix(_keywordSuffix)),
                criteria.Species)
            );
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                CellLineFilterNames.Type,
                path.Join(specimen => specimen.Cell.Type.Suffix(_keywordSuffix)),
                criteria.Type)
            );
        }

        if (IsNotEmpty(criteria.CultureType))
        {
            Add(new EqualityFilter<T, object>(
                CellLineFilterNames.CultureType,
                path.Join(specimen => specimen.Cell.CultureType.Suffix(_keywordSuffix)),
                criteria.CultureType)
            );
        }

        if (IsNotEmpty(criteria.Name))
        {
            Add(new SimilarityFilter<T, string>(
                CellLineFilterNames.Name,
                path.Join(specimen => specimen.Cell.Name),
                criteria.Name)
            );
        }
    }
}
