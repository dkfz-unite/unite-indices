using Unite.Indices.Entities.Images;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Base.Donors;
using Unite.Indices.Search.Services.Filters.Base.Images;
using Unite.Indices.Search.Services.Filters.Base.Specimens;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters;

public class ImageFiltersCollection : FiltersCollection<ImageIndex>
{
    public ImageFiltersCollection(SearchCriteria criteria) : base()
    {
        var mriFilters = new MrImageFilters<ImageIndex>(criteria.Mr, image => image.Mr);

        var donorsNavFilters = new DonorsNavFilters<ImageIndex>(criteria.Donor, image => image.Donor);
        var imagesNavFilters = new ImagesNavFilters<ImageIndex>(criteria.Image, image => image);
        var imagesDataFilters = new DataFilters<ImageIndex>(criteria.Image, image => image.Data);
        var specimensNavFilters = new SpecimensNavFilters<ImageIndex>(criteria.Specimen, image => image.Specimens.First());


        Add(mriFilters.All());

        Add(donorsNavFilters.All());
        Add(imagesNavFilters.All());
        Add(imagesDataFilters.All());
        Add(specimensNavFilters.All());
    }
}
