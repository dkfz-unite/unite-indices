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
        var imageFilters = new ImageFilters<DonorIndex>(criteria.Image, donor => donor.Images.First());
        var mriImageFilters = new MriImageFilters<DonorIndex>(criteria.Mri, donor => donor.Images.First().Mri);
        var specimenFilters = new SpecimenFilters<DonorIndex>(criteria.Specimen, donor => donor.Specimens.First());
        var materialFilters = new MaterialFilters<DonorIndex>(criteria.Material, donor => donor.Specimens.First().Material);
        var lineFilters = new LineFilters<DonorIndex>(criteria.Line, donor => donor.Specimens.First().Line);
        var organoidFilters = new OrganoidFilters<DonorIndex>(criteria.Organoid, donor => donor.Specimens.First().Organoid);
        var xenograftFilters = new XenograftFilters<DonorIndex>(criteria.Xenograft, donor => donor.Specimens.First().Xenograft);

        Add(donorFilters.All());
        Add(donorDataFilters.All());
        Add(imageFilters.All());
        Add(mriImageFilters.All());
        Add(specimenFilters.All());
        Add(materialFilters.All());
        Add(lineFilters.All());
        Add(organoidFilters.All());
        Add(xenograftFilters.All());
    }
}
