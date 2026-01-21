using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class MaterialFilters<T> : SpecimenFilters<T, MaterialIndex> where T : class
{
    protected override MaterialFilterNames FilterNames => new();

    protected override bool IncludeInterventions => false;
    protected override bool IncludeDrugScreenings => false;
    

    public MaterialFilters(MaterialCriteria criteria, Expression<Func<T, MaterialIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.FixationType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.FixationType,
                criteria.FixationType.Not,
                path.Join(specimen => specimen.FixationType.Suffix(_keywordSuffix)),
                criteria.FixationType.Value
            ));
        }

        if (IsNotEmpty(criteria.Source))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Source,
                criteria.Source.Not,
                path.Join(specimen => specimen.Source),
                criteria.Source.Value
            ));
        }
    }
}
