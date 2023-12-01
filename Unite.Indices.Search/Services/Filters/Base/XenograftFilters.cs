using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class XenograftFilters<T> : SpecimenFilters<T> where T : class
{
    public XenograftFilters(XenograftCriteria criteria, Expression<Func<T, SpecimenIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                XenograftFilterNames.ReferenceId,
                path.Join(specimen => specimen.Xenograft.ReferenceId),
                criteria.ReferenceId)
            );
        }

        if (IsNotEmpty(criteria.MouseStrain))
        {
            Add(new SimilarityFilter<T, string>(
                XenograftFilterNames.MouseStrain,
                path.Join(specimen => specimen.Xenograft.MouseStrain),
                criteria.MouseStrain)
            );
        }

        if (IsNotEmpty(criteria.Tumorigenicity))
        {
            Add(new BooleanFilter<T>(
                XenograftFilterNames.Tumorigenicity,
                path.Join(specimen => specimen.Xenograft.Tumorigenicity),
                criteria.Tumorigenicity)
            );
        }

        if (IsNotEmpty(criteria.TumorGrowthForm))
        {
            Add(new EqualityFilter<T, object>(
                XenograftFilterNames.TumorGrowthForm,
                path.Join(specimen => specimen.Xenograft.TumorGrowthForm.Suffix(_keywordSuffix)),
                criteria.TumorGrowthForm)
            );
        }

        if (IsNotEmpty(criteria.SurvivalDays))
        {
            Add(new MultiPropertyRangeFilter<T, int?>(
                XenograftFilterNames.SurvivalDays,
                path.Join(specimen => specimen.Xenograft.SurvivalDaysFrom),
                path.Join(specimen => specimen.Xenograft.SurvivalDaysTo),
                criteria.SurvivalDays?.From,
                criteria.SurvivalDays?.To)
            );
        }

        if (IsNotEmpty(criteria.Intervention))
        {
            Add(new SimilarityFilter<T, string>(
                XenograftFilterNames.Intervention,
                path.Join(specimen => specimen.Xenograft.Interventions.First().Type),
                criteria.Intervention)
            );
        }
    }
}
