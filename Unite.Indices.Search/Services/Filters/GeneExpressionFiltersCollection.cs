using Unite.Indices.Entities.Genes;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Genes;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class GeneExpressionFiltersCollection : FiltersCollection<GeneExpressionIndex>
{
    public GeneExpressionFiltersCollection(SearchCriteria criteria) : base()
    {
        var expressionFilters = new GeneExpressionFilters<GeneExpressionIndex>(criteria.Gene);
        
        var specimensNavFilters = new SpecimensNavFilters<GeneExpressionIndex>(criteria.Specimen, exp => exp.Specimen);
        var genesNavFilters = new GenesNavFilters<GeneExpressionIndex>(criteria.Gene, exp => exp.Gene);


        Add(expressionFilters.All());

        Add(specimensNavFilters.All());
        Add(genesNavFilters.All());
    }
}
