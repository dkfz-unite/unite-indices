using Unite.Indices.Entities.Genes;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class GeneFiltersCollection : FiltersCollection<GeneIndex>
{
    public GeneFiltersCollection(SearchCriteria criteria) : base()
    {
        var geneFilters = new GeneFilters<GeneIndex>(criteria.Gene, gene => gene);

        var specimensNavFilters = new SpecimensNavFilters<GeneIndex>(criteria.Specimen, gene => gene.Specimens.First());
        var genesNavFilters = new GenesNavFilters<GeneIndex>(criteria.Gene, gene => gene);
        var genesDataFilters = new GenesDataFilters<GeneIndex>(criteria.Gene, gene => gene.Data);
        
        
        Add(geneFilters.All());

        Add(specimensNavFilters.All());
        Add(genesNavFilters.All());
        Add(genesDataFilters.All());
    }
}
