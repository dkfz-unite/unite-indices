using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Indices.Search.Services.Filters;

public class OrganoidIndexFiltersCollection : SpecimenIndexFiltersCollection
{
    public OrganoidIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new OrganoidFilters<SpecimenIndex>(criteria.Organoid, specimen => specimen);
        var molecularDataFilters = new MolecularDataFilters<SpecimenIndex>(criteria.Organoid, specimen => specimen.Organoid.MolecularData);
        var drugScreeningFilters = new DrugScreeningFilters<SpecimenIndex>(criteria.Organoid, specimen => specimen.Organoid.DrugScreenings);
        var availableDataFilters = new SpecimenDataFilters<SpecimenIndex>(criteria.Organoid, specimen => specimen.Data);

        _filters.AddRange(filters.All());
        _filters.AddRange(molecularDataFilters.All());
        _filters.AddRange(drugScreeningFilters.All());
        _filters.AddRange(availableDataFilters.All());

        Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.OrganoidIndex>(
            SpecimenFilterNames.Type,
            specimen => specimen.Organoid)
        );
    }
}
