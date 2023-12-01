using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class MutationFilters<T> : VariantFilters<T> where T : class
{
    public MutationFilters(MutationCriteria criteria, Expression<Func<T, VariantIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new EqualityFilter<T, object>(
                VariantFilterNames.Chromosome,
                path.Join(variant => variant.Ssm.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome)
            );
        }

        if (IsNotEmpty(criteria.Position))
        {
            Add(new MultiPropertyRangeFilter<T, int>(
                VariantFilterNames.Position,
                path.Join(variant => variant.Ssm.Start),
                path.Join(variant => variant.Ssm.End),
                criteria.Position?.From,
                criteria.Position?.To)
            );
        }

        if (IsNotEmpty(criteria.Length))
        {
            Add(new RangeFilter<T, int>(
                VariantFilterNames.Length,
                path.Join(variant => variant.Ssm.Length),
                criteria.Length?.From,
                criteria.Length?.To)
            );
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                MutationFilterNames.Type,
                path.Join(variant => variant.Ssm.Type.Suffix(_keywordSuffix)),
                criteria.Type)
            );
        }

        if (IsNotEmpty(criteria.Impact))
        {
            Add(new EqualityFilter<T, object>(
                VariantFilterNames.Impact,
                path.Join(variant => variant.Ssm.AffectedFeatures.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
                criteria.Impact)
            );
        }

        if (IsNotEmpty(criteria.Consequence))
        {
            Add(new EqualityFilter<T, object>(
                VariantFilterNames.Consequence,
                path.Join(variant => variant.Ssm.AffectedFeatures.First().Consequences.First().Type.Suffix(_keywordSuffix)),
                criteria.Consequence)
            );
        }
    }
}
