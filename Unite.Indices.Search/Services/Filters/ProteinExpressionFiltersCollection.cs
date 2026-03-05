using Unite.Indices.Entities.Proteins;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Proteins;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class ProteinExpressionFiltersCollection : FiltersCollection<ProteinExpressionIndex>
{
    public ProteinExpressionFiltersCollection(SearchCriteria criteria) : base()
    {
        var expressionFilters = new ProteinExpressionFilters<ProteinExpressionIndex>(criteria.Protein);

        var specimensNavFilters = new SpecimensNavFilters<ProteinExpressionIndex>(criteria.Specimen, exp => exp.Specimen);
        var proteinsNavFilters = new ProteinsNavFilters<ProteinExpressionIndex>(criteria.Protein, exp => exp.Protein);

        
        Add(expressionFilters.All());

        Add(specimensNavFilters.All());
        Add(proteinsNavFilters.All());
    }
}
