using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Base.Variants;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class SsmFiltersCollection : FiltersCollection<SsmIndex>
{
    public SsmFiltersCollection(SearchCriteria criteria) : base()
    {
        var variantFilters = new SsmFilters<SsmIndex>(criteria.Ssm, variant => variant);
        var geneFilters = new GeneFilters<SsmIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);

        var specimensFilters = new SpecimensFilters<SsmIndex>(criteria.Specimen, variant => variant.Specimens.First());
        var genesFilters = new GenesFilters<SsmIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);
        var variantsFilters = new VariantsFilters<SsmIndex>(criteria.Ssm, variant => variant);

        Add(variantFilters.All());
        Add(geneFilters.All());

        Add(specimensFilters.All());
        Add(genesFilters.All());
        Add(variantsFilters.All());
    }
}
