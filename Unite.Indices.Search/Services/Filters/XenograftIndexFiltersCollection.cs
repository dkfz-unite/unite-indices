using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Indices.Search.Services.Filters;

public class XenograftIndexFiltersCollection : SpecimenIndexFiltersCollection
{
    public XenograftIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new XenograftFilters<SpecimenIndex>(criteria.Xenograft, specimen => specimen);
        var molecularDataFilters = new MolecularDataFilters<SpecimenIndex>(criteria.Xenograft, specimen => specimen.Xenograft.MolecularData);
        var drugScreeningFilters = new DrugScreeningFilters<SpecimenIndex>(criteria.Xenograft, specimen => specimen.Xenograft.DrugScreenings);
        var availableDataFilters = new SpecimenDataFilters<SpecimenIndex>(criteria.Xenograft, specimen => specimen.Data);

        _filters.AddRange(filters.All());
        _filters.AddRange(molecularDataFilters.All());
        _filters.AddRange(drugScreeningFilters.All());
        _filters.AddRange(availableDataFilters.All());

        Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.XenograftIndex>(
            SpecimenFilterNames.Type,
            specimen => specimen.Xenograft)
        );
    }
}
