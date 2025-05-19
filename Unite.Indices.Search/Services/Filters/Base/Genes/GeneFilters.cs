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
                path.Join(gene => gene.Symbol),
                criteria.Symbol
            ));
        }

        if (IsNotEmpty(criteria.Biotype))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Biotype,
                path.Join(gene => gene.Biotype.Suffix(_keywordSuffix)),
                criteria.Biotype
            ));
        }

        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Chromosome,
                path.Join(gene => gene.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome
            ));
        }

        if (IsNotEmpty(criteria.Position))
        {
            Add(new MultiPropertyRangeFilter<T, int?>(
                FilterNames.Position,
                path.Join(gene => gene.Start),
                path.Join(gene => gene.End),
                criteria.Position?.From,
                criteria.Position?.To
            ));
        }
    }
}
