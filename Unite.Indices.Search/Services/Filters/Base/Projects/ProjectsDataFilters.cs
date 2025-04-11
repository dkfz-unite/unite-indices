using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Projects.Constants;
using Unite.Indices.Search.Services.Filters.Base.Projects.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Projects;

public class ProjectsDataFilters<T> : FiltersCollection<T> where T : class
{
    protected ProjectsFilterNames FilterNames = new();

    public ProjectsDataFilters(ProjectsCriteria criteria, Expression<Func<T, DataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.HasExp))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasExp,
                path.Join(data => data.Exp),
                criteria.HasExp
            ));
        }

        if (IsNotEmpty(criteria.HasExpSc))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasExpSc,
                path.Join(data => data.ExpSc),
                criteria.HasExpSc
            ));
        }

        if (IsNotEmpty(criteria.HasSms))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasSms,
                path.Join(data => data.Sms),
                criteria.HasSms
            ));
        }

        if (IsNotEmpty(criteria.HasCnvs))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasCnvs,
                path.Join(data => data.Cnvs),
                criteria.HasCnvs
            ));
        }

        if (IsNotEmpty(criteria.HasSvs))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasSvs,
                path.Join(data => data.Svs),
                criteria.HasSvs
            ));
        }

        if (IsNotEmpty(criteria.HasMeth))
        {
            Add(new BooleanFilter<T>(
                FilterNames.HasMeth,
                path.Join(data => data.Meth),
                criteria.HasMeth
            ));
        }
    }
}
