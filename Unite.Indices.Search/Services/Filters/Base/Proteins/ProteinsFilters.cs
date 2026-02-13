using Unite.Indices.Entities.Proteins;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Constants;
using Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Proteins;

public class ProteinsFilters<T> : FiltersCollection<T> where T : ProteinIndex
{
    protected ProteinsFilterNames FilterNames = new();

    public ProteinsFilters(ProteinsCriteria criteria)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Intensity))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.Intensity,
                criteria.Intensity.Not,
                protein => protein.Specimens.First().Intensity,
                criteria.Intensity.Value?.From,
                criteria.Intensity.Value?.To
            ));
        }
    }
}
