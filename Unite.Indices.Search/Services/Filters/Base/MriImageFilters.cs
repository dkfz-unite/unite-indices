using System.Linq.Expressions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Images;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class MriImageFilters<T> : FiltersCollection<T> where T : class
{
    public MriImageFilters(in MriImageCriteria criteria, in Expression<Func<T, ImageIndex>> path) : base()
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                ImageFilterNames.Id,
                path.Join(image => image.Id),
                criteria.Id)
            );
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                MriImageFilterNames.ReferenceId,
                path.Join(image => image.Mri.ReferenceId),
                criteria.ReferenceId)
            );
        }

        if (IsNotEmpty(criteria.WholeTumor))
        {
            Add(new RangeFilter<T, double?>(
                MriImageFilterNames.WholeTumor,
                path.Join(image => image.Mri.WholeTumor),
                criteria.WholeTumor.From,
                criteria.WholeTumor.To
            ));
        }

        if (IsNotEmpty(criteria.ContrastEnhancing))
        {
            Add(new RangeFilter<T, double?>(
                MriImageFilterNames.ContrastEnhancing,
                path.Join(image => image.Mri.ContrastEnhancing),
                criteria.ContrastEnhancing.From,
                criteria.ContrastEnhancing.To
            ));
        }

        if (IsNotEmpty(criteria.NonContrastEnhancing))
        {
            Add(new RangeFilter<T, double?>(
                MriImageFilterNames.NonContrastEnhancing,
                path.Join(image => image.Mri.NonContrastEnhancing),
                criteria.NonContrastEnhancing.From,
                criteria.NonContrastEnhancing.To
            ));
        }
    }
}
