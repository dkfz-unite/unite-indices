using Nest;
using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class LineFilters<T> : SpecimenFilters<T, LineIndex> where T : class
{
    protected override LineFilterNames FilterNames => new();


    public LineFilters(LineCriteria criteria, Expression<Func<T, LineIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }
        
        if (IsNotEmpty(criteria.CellsSpecies))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.CellsSpecies,
                criteria.CellsSpecies.Not,
                path.Join(specimen => specimen.CellsSpecies.Suffix(_keywordSuffix)),
                criteria.CellsSpecies.Value
            ));
        }

        if (IsNotEmpty(criteria.CellsType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.CellsType,
                criteria.CellsType.Not,
                path.Join(specimen => specimen.CellsType.Suffix(_keywordSuffix)),
                criteria.CellsType.Value
            ));
        }

        if (IsNotEmpty(criteria.CultureType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.CellsCultureType,
                criteria.CultureType.Not,
                path.Join(specimen => specimen.CellsCultureType.Suffix(_keywordSuffix)),
                criteria.CultureType.Value
            ));
        }

        if (IsNotEmpty(criteria.Name))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Name,
                criteria.Name.Not,
                path.Join(specimen => specimen.Name),
                criteria.Name.Value
            ));
        }
    }
}
