using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Base.Variants;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class SvFiltersCollection : FiltersCollection<SvIndex>
{
    public SvFiltersCollection(SearchCriteria criteria) : base()
    {
        var variantFilters = new SvFilters<SvIndex>(criteria.Sv, variant => variant);
        var geneFilters = new GeneFilters<SvIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);

        var specimensFilters = new SpecimensFilters<SvIndex>(criteria.Specimen, variant => variant.Specimens.First());
        var genesFilters = new GenesFilters<SvIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);
        var variantsFilters = new VariantsFilters<SvIndex>(criteria.Sv, variant => variant);

        Add(variantFilters.All());
        Add(geneFilters.All());

        Add(specimensFilters.All());
        Add(genesFilters.All());
        Add(variantsFilters.All());
    }
}
