using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class MolecularDataFilters<T> : FiltersCollection<T> where T : class
{
    public MolecularDataFilters(SpecimenCriteriaBase criteria, Expression<Func<T, MolecularDataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.MgmtStatus))
        {
            Add(new EqualityFilter<T, object>(
                SpecimenFilterNames.MgmtStatus,
                path.Join(molecularData => molecularData.MgmtStatus.Suffix(_keywordSuffix)),
                criteria.MgmtStatus)
            );
        }

        if (IsNotEmpty(criteria.IdhStatus))
        {
            Add(new EqualityFilter<T, object>(
                SpecimenFilterNames.IdhStatus,
                path.Join(molecularData => molecularData.IdhStatus.Suffix(_keywordSuffix)),
                criteria.IdhStatus)
            );
        }

        if (IsNotEmpty(criteria.IdhMutation))
        {
            Add(new EqualityFilter<T, object>(
                SpecimenFilterNames.IdhMutation,
                path.Join(molecularData => molecularData.IdhMutation.Suffix(_keywordSuffix)),
                criteria.IdhMutation)
            );
        }

        if (IsNotEmpty(criteria.GeneExpressionSubtype))
        {
            Add(new EqualityFilter<T, object>(
                SpecimenFilterNames.GeneExpressionSubtype,
                path.Join(molecularData => molecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
                criteria.GeneExpressionSubtype)
            );
        }

        if (IsNotEmpty(criteria.MethylationSubtype))
        {
            Add(new EqualityFilter<T, object>(
                SpecimenFilterNames.MethylationSubtype,
                path.Join(molecularData => molecularData.MethylationSubtype.Suffix(_keywordSuffix)),
                criteria.MethylationSubtype)
            );
        }

        if (IsNotEmpty(criteria.GcimpMethylation))
        {
            Add(new BooleanFilter<T>(
                SpecimenFilterNames.GcimpMethylation,
                path.Join(molecularData => molecularData.GcimpMethylation),
                criteria.GcimpMethylation)
            );
        }
    }
}
