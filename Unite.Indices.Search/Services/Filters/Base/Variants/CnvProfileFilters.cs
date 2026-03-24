using Unite.Indices.Entities.CnvProfiles;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public class CnvProfileFilters : FiltersCollection<CnvProfileIndex>
{
    public CnvProfileFilters(CnvProfileCriteria criteria)
    {
        if (criteria == null)
        {
            return;
        }
        
        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new SimilarityFilter<CnvProfileIndex, object>(
                "cnvp.chromosome",
                criteria.Chromosome.Not,
                cnvProfile => cnvProfile.Chromosome,
                criteria.Chromosome.Value
            ));
        }
    }
}