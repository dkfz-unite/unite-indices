using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Base.Proteins;
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
        var proteinFilters = new ProteinFilters<SvIndex>(criteria.Protein, variant => variant.AffectedFeatures.First().Transcript.Feature.Protein);

        var specimensNavFilters = new SpecimensNavFilters<SvIndex>(criteria.Specimen, variant => variant.Specimens.First());
        var variantsNavFilters = new VariantsNavFilters<SvIndex>(criteria.Sv, variant => variant);
        var genesNavFilters = new GenesNavFilters<SvIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);
        var proteinsNavFilters = new ProteinsNavFilters<SvIndex>(criteria.Protein, variant => variant.AffectedFeatures.First().Transcript.Feature.Protein);
        

        Add(variantFilters.All());
        Add(geneFilters.All());

        Add(specimensNavFilters.All());
        Add(variantsNavFilters.All());
        Add(genesNavFilters.All());
        Add(proteinsNavFilters.All());
    }
}
