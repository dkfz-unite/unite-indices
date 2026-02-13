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

        var donorsNavFilters = new DonorsNavFilters<DonorIndex>(criteria.Donor, donor => donor);
        var donorsDataFilters = new DataFilters<DonorIndex>(criteria.Donor, donor => donor.Data);
        var imagesNavFilters = new ImagesNavFilters<DonorIndex>(criteria.Image, donor => donor.Images.First());
        var specimensNavFilters = new SpecimensNavFilters<DonorIndex>(criteria.Specimen, donor => donor.Specimens.First());


        Add(donorFilters.All());

        Add(donorsNavFilters.All());
        Add(donorsDataFilters.All());
        Add(imagesNavFilters.All());
        Add(specimensNavFilters.All());
    }
}
