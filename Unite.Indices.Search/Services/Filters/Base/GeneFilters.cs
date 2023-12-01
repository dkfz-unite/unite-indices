using System.Linq.Expressions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Genome;
using Unite.Essentials.Extensions;
using Nest;

namespace Unite.Indices.Search.Services.Filters.Base;

public class GeneFilters<T> : FiltersCollection<T> where T : class
{
    public GeneFilters(GeneCriteria criteria, Expression<Func<T, GeneIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                GeneFilterNames.Id,
                path.Join(gene => gene.Id),
                criteria.Id)
            );
        }

        if (IsNotEmpty(criteria.Symbol))
        {
            Add(new SimilarityFilter<T, string>(
                GeneFilterNames.Symbol,
                path.Join(gene => gene.Symbol),
                criteria.Symbol)
            );
        }

        if (IsNotEmpty(criteria.Biotype))
        {
            Add(new EqualityFilter<T, object>(
                GeneFilterNames.Biotype,
                path.Join(gene => gene.Biotype.Suffix(_keywordSuffix)),
                criteria.Biotype)
            );
        }

        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new EqualityFilter<T, object>(
                GeneFilterNames.Chromosome,
                path.Join(gene => gene.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome)
            );
        }

        if (IsNotEmpty(criteria.Position))
        {
            Add(new MultiPropertyRangeFilter<T, int?>(
                GeneFilterNames.Position,
                path.Join(gene => gene.Start),
                path.Join(gene => gene.End),
                criteria.Position?.From,
                criteria.Position?.To)
            );
        }
    }
}
