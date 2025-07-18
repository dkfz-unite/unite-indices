﻿using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class XenograftFilters<T> : SpecimenFilters<T, XenograftIndex> where T : class
{
    protected override XenograftFilterNames FilterNames => new();
    

    public XenograftFilters(XenograftCriteria criteria, Expression<Func<T, XenograftIndex>> path) : base(criteria, path)
    {
        if (criteria == null)
        {
            return;
        }
        
        if (IsNotEmpty(criteria.MouseStrain))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.MouseStrain,
                criteria.MouseStrain.Not,
                path.Join(specimen => specimen.MouseStrain),
                criteria.MouseStrain.Value
            ));
        }

        if (IsNotEmpty(criteria.Tumorigenicity))
        {
            Add(new BooleanFilter<T>(
                FilterNames.Tumorigenicity,
                criteria.Tumorigenicity.Not,
                path.Join(specimen => specimen.Tumorigenicity),
                criteria.Tumorigenicity.Value
            ));
        }

        if (IsNotEmpty(criteria.TumorGrowthForm))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.TumorGrowthForm,
                criteria.TumorGrowthForm.Not,
                path.Join(specimen => specimen.TumorGrowthForm.Suffix(_keywordSuffix)),
                criteria.TumorGrowthForm.Value
            ));
        }

        if (IsNotEmpty(criteria.SurvivalDays))
        {
            Add(new MultiPropertyRangeFilter<T, int?>(
                FilterNames.SurvivalDays,
                criteria.SurvivalDays.Not,
                path.Join(specimen => specimen.SurvivalDaysFrom),
                path.Join(specimen => specimen.SurvivalDaysTo),
                criteria.SurvivalDays?.Value.From,
                criteria.SurvivalDays?.Value.To
            ));
        }
    }
}
