using Unite.Indices.Entities.Genes;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Genes.Constants;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Genes;

public class GenesFilters<T> : FiltersCollection<T> where T : GeneIndex
{
    protected GenesFilterNames FilterNames = new();

    public GenesFilters(GenesCriteria criteria)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.TPM))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.TPM,
                criteria.TPM.Not,
                gene => gene.Specimens.First().TPM,
                criteria.TPM.Value?.From,
                criteria.TPM.Value?.To
            ));
        }

        if (IsNotEmpty(criteria.FPKM))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.FPKM,
                criteria.FPKM.Not,
                gene => gene.Specimens.First().FPKM,
                criteria.FPKM.Value?.From,
                criteria.FPKM.Value?.To
            ));
        }
    }
}
