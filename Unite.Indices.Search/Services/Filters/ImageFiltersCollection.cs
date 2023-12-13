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
        var donorFilters = new DonorFilters<ImageIndex>(criteria.Donor, image => image.Donor);
        var imageFilters = new ImageFilters<ImageIndex>(criteria.Image, image => image);
        var imageDataFilters = new ImageDataFilters<ImageIndex>(criteria.Image, image => image.Data);
        var mriImageFilters = new MriImageFilters<ImageIndex>(criteria.Mri, image => image.Mri);
        var specimenFilters = new SpecimenFilters<ImageIndex>(criteria.Specimen, image => image.Specimens.First());
        var tissueFilters = new TissueFilters<ImageIndex>(criteria.Tissue, image => image.Specimens.First().Tissue);

        Add(donorFilters.All());
        Add(imageFilters.All());
        Add(imageDataFilters.All());
        Add(mriImageFilters.All());
        Add(specimenFilters.All());
        Add(tissueFilters.All());
    }
}
