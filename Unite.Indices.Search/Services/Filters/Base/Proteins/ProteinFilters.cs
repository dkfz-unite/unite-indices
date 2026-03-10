using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Omics;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Constants;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Proteins;

public class ProteinFilters<T>: FiltersCollection<T> where T : class
{
    protected ProteinFilterNames FilterNames = new();

    public ProteinFilters(ProteinCriteria criteria, Expression<Func<T, ProteinIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Accession))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Accession,
                criteria.Accession.Not,
                path.Join(protein => protein.AccessionId),
                criteria.Accession.Value
            ));
        }

        if (IsNotEmpty(criteria.Symbol))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Symbol,
                criteria.Symbol.Not,
                path.Join(protein => protein.Symbol),
                criteria.Symbol.Value
            ));
        }

        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Chromosome,
                criteria.Chromosome.Not,
                path.Join(protein => protein.Chromosome.Suffix(_keywordSuffix)),
                criteria.Chromosome.Value
            ));
        }

        if (IsNotEmpty(criteria.Position))
        {
            Add(new MultiPropertyRangeFilter<T, int?>(
                FilterNames.Position,
                criteria.Position.Not,
                path.Join(protein => protein.Start),
                path.Join(protein => protein.End),
                criteria.Position.Value?.From,
                criteria.Position.Value?.To
            ));
        }
    }
}
