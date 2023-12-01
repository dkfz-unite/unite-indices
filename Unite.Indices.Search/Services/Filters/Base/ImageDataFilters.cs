using System.Linq.Expressions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Images;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class ImageDataFilters<T> : FiltersCollection<T> where T : class
{
    public ImageDataFilters(ImageCriteriaBase criteria, Expression<Func<T, DataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.HasSsms))
        {
            Add(new BooleanFilter<T>(
                ImageFilterNames.HasSsms,
                path.Join(data => data.Ssms),
                criteria.HasSsms)
            );
        }

        if (IsNotEmpty(criteria.HasCnvs))
        {
            Add(new BooleanFilter<T>(
                ImageFilterNames.HasCnvs,
                path.Join(data => data.Cnvs),
                criteria.HasCnvs)
            );
        }

        if (IsNotEmpty(criteria.HasSvs))
        {
            Add(new BooleanFilter<T>(
                ImageFilterNames.HasSvs,
                path.Join(data => data.Svs),
                criteria.HasSvs)
            );
        }

        if (IsNotEmpty(criteria.HasGeneExp))
        {
            Add(new BooleanFilter<T>(
                ImageFilterNames.HasGeneExp,
                path.Join(data => data.GeneExp),
                criteria.HasGeneExp)
            );
        }
    }
}
