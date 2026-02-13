using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Base.Proteins;
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
        var proteinFilters = new ProteinFilters<SmIndex>(criteria.Protein, variant => variant.AffectedFeatures.First().Transcript.Feature.Protein);

        var specimensNavFilters = new SpecimensNavFilters<SmIndex>(criteria.Specimen, variant => variant.Specimens.First());
        var variantsNavFilters = new VariantsNavFilters<SmIndex>(criteria.Sm, variant => variant);
        var genesNavFilters = new GenesNavFilters<SmIndex>(criteria.Gene, variant => variant.AffectedFeatures.First().Gene);
        var proteinsNavFilters = new ProteinsNavFilters<SmIndex>(criteria.Protein, variant => variant.AffectedFeatures.First().Transcript.Feature.Protein);
        

        Add(variantFilters.All());
        Add(geneFilters.All());
        Add(proteinFilters.All());

        Add(specimensNavFilters.All());
        Add(variantsNavFilters.All());
        Add(genesNavFilters.All());
        Add(proteinsNavFilters.All());
    }
}
