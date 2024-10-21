using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Genome.Dna;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Constants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public abstract class VariantBaseFilters<T, TModel> : FiltersCollection<T> 
    where T : class
    where TModel : VariantBaseIndex
{
    protected abstract VariantFilterNames FilterNames { get; }

    public VariantBaseFilters(VariantBaseCriteria criteria, Expression<Func<T, TModel>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Id,
                path.Join(variant => variant.Id.Suffix(_keywordSuffix)),
                criteria.Id
            ));
        }

        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Chromosome,
                path.Join(variant => variant.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome
            ));
        }

        if (IsNotEmpty(criteria.Position))
        {
            Add(new MultiPropertyRangeFilter<T, int>(
                FilterNames.Position,
                path.Join(variant => variant.Start),
                path.Join(variant => variant.End),
                criteria.Position?.From,
                criteria.Position?.To
            ));
        }

        if (IsNotEmpty(criteria.Length))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.Length,
                path.Join(variant => variant.Length),
                criteria.Length?.From,
                criteria.Length?.To
            ));
        }

        if (IsNotEmpty(criteria.Gene))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Gene,
                path.Join(variant => variant.AffectedFeatures.First().Gene.Symbol.Suffix(_keywordSuffix)),
                criteria.Gene
            ));
        }

        if (IsNotEmpty(criteria.Impact))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Impact,
                path.Join(variant => variant.AffectedFeatures.First().Effects.First().Impact.Suffix(_keywordSuffix)),
                criteria.Impact
            ));
        }

        if (IsNotEmpty(criteria.Effect))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Effect,
                path.Join(variant => variant.AffectedFeatures.First().Effects.First().Type.Suffix(_keywordSuffix)),
                criteria.Effect
            ));
        }
    }
}
