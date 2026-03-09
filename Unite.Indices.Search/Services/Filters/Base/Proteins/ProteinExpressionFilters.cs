using Unite.Indices.Entities.Proteins;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Constants;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Proteins;

public class ProteinExpressionFilters<T> : FiltersCollection<T> where T : ProteinExpressionIndex
{
    protected ProteinsFilterNames FilterNames = new();

    public ProteinExpressionFilters(ProteinsCriteria criteria)
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
                protein => protein.Expression,
                criteria.Expression.Value?.From,
                criteria.Expression.Value?.To
            ));
        }
    }
}
