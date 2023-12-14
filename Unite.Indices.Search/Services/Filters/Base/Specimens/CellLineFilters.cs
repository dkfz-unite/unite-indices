using Nest;
using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class CellLineFilters<T> : SpecimenBaseFilters<T, CellLineIndex> where T : class
{
    protected override CellLineFilterNames FilterNames => new();

    public CellLineFilters(CellLineCriteria criteria, Expression<Func<T, CellLineIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }
        
        if (IsNotEmpty(criteria.Species))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Species,
                path.Join(specimen => specimen.Species.Suffix(_keywordSuffix)),
                criteria.Species
            ));
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Type,
                path.Join(specimen => specimen.Type.Suffix(_keywordSuffix)),
                criteria.Type
            ));
        }

        if (IsNotEmpty(criteria.CultureType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.CultureType,
                path.Join(specimen => specimen.CultureType.Suffix(_keywordSuffix)),
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
