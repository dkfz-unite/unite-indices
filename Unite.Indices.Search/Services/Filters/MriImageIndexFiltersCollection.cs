using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Images;

namespace Unite.Indices.Search.Services.Filters;

public class MriImageIndexFiltersCollection : ImageIndexFiltersCollection
{
    public MriImageIndexFiltersCollection(SearchCriteria criteria) : base(criteria)
    {
        var filters = new MriImageFilters<ImageIndex>(criteria.Mri, image => image);
        var availableDataFilters = new ImageDataFilters<ImageIndex>(criteria.Mri, image => image.Data);

        _filters.AddRange(filters.All());
        _filters.AddRange(availableDataFilters.All());

        Add(new NotNullFilter<ImageIndex, Indices.Entities.Basic.Images.MriImageIndex>(
            ImageFilterNames.Type,
            image => image.Mri)
        );
    }
}
