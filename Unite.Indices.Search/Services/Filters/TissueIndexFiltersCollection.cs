using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Indices.Search.Services.Filters;

public class TissueIndexFiltersCollection : SpecimenIndexFiltersCollection
{
    public TissueIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new TissueFilters<SpecimenIndex>(criteria.Tissue, specimen => specimen);
        var molecularDataFilters = new MolecularDataFilters<SpecimenIndex>(criteria.Tissue, specimen => specimen.Tissue.MolecularData);
        var availableDataFilters = new SpecimenDataFilters<SpecimenIndex>(criteria.Tissue, specimen => specimen.Data);

        _filters.AddRange(filters.All());
        _filters.AddRange(molecularDataFilters.All());
        _filters.AddRange(availableDataFilters.Except(SpecimenFilterNames.HasDrugs));

        Add(new NotNullFilter<SpecimenIndex, Indices.Entities.Basic.Specimens.TissueIndex>(
            SpecimenFilterNames.Type,
            specimen => specimen.Tissue)
        );
    }
}
