using Nest;
using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class LineFilters<T> : SpecimenBaseFilters<T, LineIndex> where T : class
{
    protected override LineFilterNames FilterNames => new();

    protected override bool IncludeInterventions => false;


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
                path.Join(specimen => specimen.CellsSpecies.Suffix(_keywordSuffix)),
                criteria.CellsSpecies
            ));
        }

        if (IsNotEmpty(criteria.CellsType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.CellsType,
                path.Join(specimen => specimen.CellsType.Suffix(_keywordSuffix)),
                criteria.CellsType
            ));
        }

        if (IsNotEmpty(criteria.CultureType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.CellsCultureType,
                path.Join(specimen => specimen.CellsCultureType.Suffix(_keywordSuffix)),
                criteria.CultureType
            ));
        }

        if (IsNotEmpty(criteria.Name))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Name,
                path.Join(specimen => specimen.Name),
                criteria.Name
            ));
        }
    }
}
