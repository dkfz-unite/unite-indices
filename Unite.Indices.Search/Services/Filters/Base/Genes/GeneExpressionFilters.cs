using Unite.Indices.Entities.Genes;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Genes.Constants;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Genes;

public class GeneExpressionFilters<T> : FiltersCollection<T> where T : GeneExpressionIndex
{
    protected GenesFilterNames FilterNames = new();

    public GeneExpressionFilters(GenesCriteria criteria)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Expression))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.Expression,
                criteria.Expression.Not,
                gene => gene.Expression,
                criteria.Expression.Value?.From,
                criteria.Expression.Value?.To
            ));
        }
    }
}
