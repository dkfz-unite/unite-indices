using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Base.Variants;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class SmFiltersCollection : FiltersCollection<SmIndex>
{
    public SmFiltersCollection(SearchCriteria criteria) : base()
    {
        var variantFilters = new SmFilters<SmIndex>(criteria.Sm, variant => variant);
        var geneFilters = new GeneFilters<SmIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);

        var specimensFilters = new SpecimensFilters<SmIndex>(criteria.Specimen, variant => variant.Specimens.First());
        var genesFilters = new GenesFilters<SmIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);
        var variantsFilters = new VariantsFilters<SmIndex>(criteria.Sm, variant => variant);

        Add(variantFilters.All());
        Add(geneFilters.All());

        Add(specimensFilters.All());
        Add(genesFilters.All());
        Add(variantsFilters.All());
    }
}
