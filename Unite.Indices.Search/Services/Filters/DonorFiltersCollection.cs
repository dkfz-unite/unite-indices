using Unite.Indices.Entities.Donors;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Donors;
using Unite.Indices.Search.Services.Filters.Base.Images;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class DonorFiltersCollection : FiltersCollection<DonorIndex>
{
    public DonorFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<DonorIndex>(criteria.Donor, donor => donor);
        var donorDataFilters = new DonorDataFilters<DonorIndex>(criteria.Donor, donor => donor.Data);
        var mriImageFilters = new MriImageFilters<DonorIndex>(criteria.Mri, donor => donor.Images.First().Mri);
        var tissueFilters = new TissueFilters<DonorIndex>(criteria.Tissue, donor => donor.Specimens.First().Tissue);
        var cellLineFilters = new CellLineFilters<DonorIndex>(criteria.Cell, donor => donor.Specimens.First().Cell);
        var organoidFilters = new OrganoidFilters<DonorIndex>(criteria.Organoid, donor => donor.Specimens.First().Organoid);
        var xenograftFilters = new XenograftFilters<DonorIndex>(criteria.Xenograft, donor => donor.Specimens.First().Xenograft);

        Add(donorFilters.All());
        Add(donorDataFilters.All());
        Add(mriImageFilters.All());
        Add(tissueFilters.All());
        Add(cellLineFilters.All());
        Add(organoidFilters.All());
        Add(xenograftFilters.All());
    }
}
