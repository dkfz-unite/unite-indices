using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class TissueFilters<T> : SpecimenBaseFilters<T, TissueIndex> where T : class
{
    protected override TissueFilterNames FilterNames => new();
    protected override bool IncludeDrugScreenings => false;

    public TissueFilters(TissueCriteria criteria, Expression<Func<T, TissueIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }
        
        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Type,
                path.Join(specimen => specimen.Type.Suffix(_keywordSuffix)),
                criteria.Type
            ));
        }

        if (IsNotEmpty(criteria.TumorType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.TumorType,
                path.Join(specimen => specimen.TumorType.Suffix(_keywordSuffix)),
                criteria.TumorType
            ));
        }

        if (IsNotEmpty(criteria.Source))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Source,
                path.Join(specimen => specimen.Source),
                criteria.Source
            ));
        }
    }
}
