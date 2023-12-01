using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome.Variants;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class CopyNumberVariantFilters<T> : VariantFilters<T> where T : class
{
    public CopyNumberVariantFilters(CopyNumberVariantCriteria criteria, Expression<Func<T, VariantIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new EqualityFilter<T, object>(
                VariantFilterNames.Chromosome,
                path.Join(variant => variant.Cnv.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome)
            );
        }

        if (IsNotEmpty(criteria.Position))
        {
            Add(new MultiPropertyRangeFilter<T, int>(
                VariantFilterNames.Position,
                path.Join(variant => variant.Cnv.Start),
                path.Join(variant => variant.Cnv.End),
                criteria.Position?.From,
                criteria.Position?.To)
            );
        }

        if (IsNotEmpty(criteria.Length))
        {
            Add(new RangeFilter<T, int>(
                VariantFilterNames.Length,
                path.Join(variant => variant.Cnv.Length),
                criteria.Length?.From,
                criteria.Length?.To)
            );
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                CopyNumberVariantFilterNames.Type,
                path.Join(variant => variant.Cnv.Type.Suffix(_keywordSuffix)),
                criteria.Type)
            );
        }

        if (IsNotEmpty(criteria.Loh))
        {
            Add(new BooleanFilter<T>(
                CopyNumberVariantFilterNames.Loh,
                path.Join(variant => variant.Cnv.Loh),
                criteria.Loh)
            );
        }

        if (IsNotEmpty(criteria.HomoDel))
        {
            Add(new BooleanFilter<T>(
                CopyNumberVariantFilterNames.HomoDel,
                path.Join(variant => variant.Cnv.HomoDel),
                criteria.HomoDel)
            );
        }

        if (IsNotEmpty(criteria.Impact))
        {
            Add(new EqualityFilter<T, object>(
                VariantFilterNames.Impact,
                path.Join(variant => variant.Cnv.AffectedFeatures.First().Consequences.First().Impact.Suffix(_keywordSuffix)),
                criteria.Impact)
            );
        }

        if (IsNotEmpty(criteria.Consequence))
        {
            Add(new EqualityFilter<T, object>(
                VariantFilterNames.Consequence,
                path.Join(variant => variant.Cnv.AffectedFeatures.First().Consequences.First().Type.Suffix(_keywordSuffix)),
                criteria.Consequence)
            );
        }
    }
}
