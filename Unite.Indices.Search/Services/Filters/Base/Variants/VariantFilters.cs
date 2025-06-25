using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Omics.Variants;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Constants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public abstract class VariantFilters<T, TModel> : FiltersCollection<T> 
    where T : class
    where TModel : VariantBaseIndex
{
    protected abstract VariantFilterNames FilterNames { get; }

    public VariantFilters(VariantCriteria criteria, Expression<Func<T, TModel>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Chromosome,
                path.Join(variant => variant.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome.Value
            ));
        }

        if (IsNotEmpty(criteria.Position))
        {
            Add(new MultiPropertyRangeFilter<T, int>(
                FilterNames.Position,
                path.Join(variant => variant.Start),
                path.Join(variant => variant.End),
                criteria.Position.Value?.From,
                criteria.Position.Value?.To
            ));
        }

        if (IsNotEmpty(criteria.Length))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.Length,
                path.Join(variant => variant.Length),
                criteria.Length.Value?.From,
                criteria.Length.Value?.To
            ));
        }

        if (IsNotEmpty(criteria.Gene))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Gene,
                path.Join(variant => variant.AffectedFeatures.First().Gene.Symbol.Suffix(_keywordSuffix)),
                criteria.Gene.Value
            ));
        }

        if (IsNotEmpty(criteria.Impact))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Impact,
                path.Join(variant => variant.AffectedFeatures.First().Effects.First().Impact.Suffix(_keywordSuffix)),
                criteria.Impact.Value
            ));
        }

        if (IsNotEmpty(criteria.Effect))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Effect,
                path.Join(variant => variant.AffectedFeatures.First().Effects.First().Type.Suffix(_keywordSuffix)),
                criteria.Effect.Value
            ));
        }
    }
}
