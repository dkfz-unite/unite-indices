using Nest;
using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Omics;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Genes.Constants;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Genes;

public class GeneFilters<T> : FiltersCollection<T> where T : class
{
    protected GeneFilterNames FilterNames = new();

    public GeneFilters(GeneCriteria criteria, Expression<Func<T, GeneIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Symbol))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Symbol,
                criteria.Symbol.Not,
                path.Join(gene => gene.Symbol),
                criteria.Symbol.Value
            ));
        }

        if (IsNotEmpty(criteria.Biotype))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Biotype,
                criteria.Biotype.Not,
                path.Join(gene => gene.Biotype.Suffix(_keywordSuffix)),
                criteria.Biotype.Value
            ));
        }

        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Chromosome,
                criteria.Chromosome.Not,
                path.Join(gene => gene.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome.Value
            ));
        }

        if (IsNotEmpty(criteria.Position))
        {
            Add(new MultiPropertyRangeFilter<T, int?>(
                FilterNames.Position,
                criteria.Position.Not,
                path.Join(gene => gene.Start),
                path.Join(gene => gene.End),
                criteria.Position.Value?.From,
                criteria.Position.Value?.To
            ));
        }
    }
}
