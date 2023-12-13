using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class XenograftFilters<T> : SpecimenBaseFilters<T, XenograftIndex> where T : class
{
    protected override XenograftFilterNames FilterNames => new();

    public XenograftFilters(XenograftCriteria criteria, Expression<Func<T, XenograftIndex>> path) : base(criteria, path)
    {
        if (IsNotEmpty(criteria.MouseStrain))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.MouseStrain,
                path.Join(specimen => specimen.MouseStrain),
                criteria.MouseStrain
            ));
        }

        if (IsNotEmpty(criteria.Tumorigenicity))
        {
            Add(new BooleanFilter<T>(
                FilterNames.Tumorigenicity,
                path.Join(specimen => specimen.Tumorigenicity),
                criteria.Tumorigenicity
            ));
        }

        if (IsNotEmpty(criteria.TumorGrowthForm))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.TumorGrowthForm,
                path.Join(specimen => specimen.TumorGrowthForm.Suffix(_keywordSuffix)),
                criteria.TumorGrowthForm
            ));
        }

        if (IsNotEmpty(criteria.SurvivalDays))
        {
            Add(new MultiPropertyRangeFilter<T, int?>(
                FilterNames.SurvivalDays,
                path.Join(specimen => specimen.SurvivalDaysFrom),
                path.Join(specimen => specimen.SurvivalDaysTo),
                criteria.SurvivalDays?.From,
                criteria.SurvivalDays?.To
            ));
        }

        if (IsNotEmpty(criteria.Intervention))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Intervention,
                path.Join(specimen => specimen.Interventions.First().Type),
                criteria.Intervention
            ));
        }
    }
}
