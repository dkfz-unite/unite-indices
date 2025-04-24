using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Images;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Images.Constants;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Images;

public abstract class ImageFilters<T, TModel> : FiltersCollection<T>
    where T : class
    where TModel : ImageBaseIndex
{
    protected abstract ImageFilterNames FilterNames { get; }

    public ImageFilters(ImageCriteria criteria, Expression<Func<T, TModel>> path)
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
                criteria.Id
            ));
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.ReferenceId,
                path.Join(image => image.ReferenceId),
                criteria.ReferenceId
            ));
        }
    }
}
