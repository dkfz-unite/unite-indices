using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Images;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Images.Constants;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Images;

public class MrImageFilters<T> : ImageFilters<T, MrImageIndex> where T : class
{
    protected override MrImageFilterNames FilterNames => new();

    public MrImageFilters(in MrImageCriteria criteria, in Expression<Func<T, MrImageIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.WholeTumor))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.WholeTumor,
                criteria.WholeTumor.Not,
                path.Join(image => image.WholeTumor),
                criteria.WholeTumor.Value?.From,
                criteria.WholeTumor.Value?.To
            ));
        }

        if (IsNotEmpty(criteria.ContrastEnhancing))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.ContrastEnhancing,
                criteria.ContrastEnhancing.Not,
                path.Join(image => image.ContrastEnhancing),
                criteria.ContrastEnhancing?.Value.From,
                criteria.ContrastEnhancing?.Value.To
            ));
        }

        if (IsNotEmpty(criteria.NonContrastEnhancing))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.NonContrastEnhancing,
                criteria.NonContrastEnhancing.Not,
                path.Join(image => image.NonContrastEnhancing),
                criteria.NonContrastEnhancing?.Value.From,
                criteria.NonContrastEnhancing?.Value.To
            ));
        }
    }
}
