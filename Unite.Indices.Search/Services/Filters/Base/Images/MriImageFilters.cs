using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Images;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Images.Constants;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Images;

public class MriImageFilters<T> : ImageBaseFilters<T, MriImageIndex> where T : class
{
    protected override MriImageFilterNames FilterNames => new();

    public MriImageFilters(in MriImageCriteria criteria, in Expression<Func<T, MriImageIndex>> path) : base(criteria, path)
    {
        if (IsNotEmpty(criteria.WholeTumor))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.WholeTumor,
                path.Join(image => image.WholeTumor),
                criteria.WholeTumor.From,
                criteria.WholeTumor.To
            ));
        }

        if (IsNotEmpty(criteria.ContrastEnhancing))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.ContrastEnhancing,
                path.Join(image => image.ContrastEnhancing),
                criteria.ContrastEnhancing.From,
                criteria.ContrastEnhancing.To
            ));
        }

        if (IsNotEmpty(criteria.NonContrastEnhancing))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.NonContrastEnhancing,
                path.Join(image => image.NonContrastEnhancing),
                criteria.NonContrastEnhancing.From,
                criteria.NonContrastEnhancing.To
            ));
        }
    }
}
