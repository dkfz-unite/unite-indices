using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Base.Variants;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class CnvFiltersCollection : FiltersCollection<CnvIndex>
{
    public CnvFiltersCollection(SearchCriteria criteria) : base()
    {
        var variantFilters = new CnvFilters<CnvIndex>(criteria.Cnv, variant => variant);
        var geneFilters = new GeneFilters<CnvIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);

        var specimensFilters = new SpecimensFilters<CnvIndex>(criteria.Specimen, variant => variant.Specimens.First());
        var variantsFilters = new VariantsFilters<CnvIndex>(criteria.Cnv, variant => variant);
        var genesFilters = new GenesFilters<CnvIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);
        

        Add(variantFilters.All());
        Add(geneFilters.All());

        Add(specimensFilters.All());
        Add(variantsFilters.All());
        Add(genesFilters.All());
    }
}
