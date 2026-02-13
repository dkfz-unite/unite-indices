using Unite.Indices.Entities.Proteins;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Proteins;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class ProteinFiltersCollection : FiltersCollection<ProteinIndex>
{
    public ProteinFiltersCollection(SearchCriteria criteria) : base()
    {
        var proteinFilters = new ProteinFilters<ProteinIndex>(criteria.Protein, protein => protein);
        var proteinsFilters = new ProteinsFilters<ProteinIndex>(criteria.Protein);

        var specimensNavFilters = new SpecimensNavFilters<ProteinIndex>(criteria.Specimen, protein => protein.Specimens.First());
        var proteinsNavFilters = new ProteinsNavFilters<ProteinIndex>(criteria.Protein, protein => protein);        
        
        Add(proteinFilters.All());
        Add(proteinsFilters.All());

        Add(specimensNavFilters.All());
        Add(proteinsNavFilters.All());
    }
}
