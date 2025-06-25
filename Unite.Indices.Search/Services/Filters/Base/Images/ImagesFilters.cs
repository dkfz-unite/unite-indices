using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Images;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Images.Constants;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Images;

public class ImagesFilters<T> : FiltersCollection<T> where T : class
{
    protected ImagesFilterNames FilterNames = new();

    public ImagesFilters(ImagesCriteria criteria, Expression<Func<T, ImageNavIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                FilterNames.Id,
                path.Join(image => image.Id),
                criteria.Id.Value
            ));
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.ReferenceId,
                path.Join(donor => donor.ReferenceId),
                criteria.ReferenceId.Value
            ));
        }

        if (IsNotEmpty(criteria.ImageType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.ImageType,
                path.Join(image => image.Type.Suffix(_keywordSuffix)),
                criteria.ImageType.Value
            ));
        }
    }
}
