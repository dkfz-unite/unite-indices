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

        var donorsFilters = new DonorsFilters<ImageIndex>(criteria.Donor, image => image.Donor);
        var imagesFilters = new ImagesFilters<ImageIndex>(criteria.Image, image => image);
        var imagesDataFilters = new ImagesDataFilters<ImageIndex>(criteria.Image, image => image.Data);
        var specimensFilters = new SpecimensFilters<ImageIndex>(criteria.Specimen, image => image.Specimens.First());


        Add(mriFilters.All());

        Add(donorsFilters.All());
        Add(imagesFilters.All());
        Add(imagesDataFilters.All());
        Add(specimensFilters.All());
    }
}
