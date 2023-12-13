using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Images;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Images.Constants;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Images;

public class ImageFilters<T> : FiltersCollection<T> where T : class
{
    public ImageFilters(ImageCriteria criteria, Expression<Func<T, ImageIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                ImageFilterNames.ImageId,
                path.Join(image => image.Id),
                criteria.Id
            ));
        }

        if (IsNotEmpty(criteria.Type))
        {
            Add(new EqualityFilter<T, object>(
                ImageFilterNames.ImageType,
                path.Join(image => image.Type.Suffix(_keywordSuffix)),
                criteria.Type
            ));
        }
    }
}
