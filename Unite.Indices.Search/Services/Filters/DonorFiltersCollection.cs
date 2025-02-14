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

        var donorsFilters = new DonorsFilters<DonorIndex>(criteria.Donor, donor => donor);
        var donorsDataFilters = new DonorsDataFilters<DonorIndex>(criteria.Donor, donor => donor.Data);
        var imagesFilters = new ImagesFilters<DonorIndex>(criteria.Image, donor => donor.Images.First());
        var specimensFilters = new SpecimensFilters<DonorIndex>(criteria.Specimen, donor => donor.Specimens.First());


        Add(donorFilters.All());

        Add(donorsFilters.All());
        Add(donorsDataFilters.All());
        Add(imagesFilters.All());
        Add(specimensFilters.All());
    }
}
