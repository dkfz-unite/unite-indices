using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Omics;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Constants;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Proteins;

public class ProteinsNavFilters<T> : FiltersCollection<T> where T : class
{
    protected ProteinsFilterNames FilterNames = new();

    public ProteinsNavFilters(ProteinsCriteria criteria, Expression<Func<T, ProteinNavIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                FilterNames.Id,
                criteria.Id.Not,
                path.Join(protein => protein.Id),
                criteria.Id.Value
            ));
        }
    }
}
