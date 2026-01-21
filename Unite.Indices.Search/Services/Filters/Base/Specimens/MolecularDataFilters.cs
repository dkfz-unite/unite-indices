using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class MolecularDataFilters<T> : FiltersCollection<T> where T : class
{
    protected SpecimenFilterNames FilterNames = new();

    public MolecularDataFilters(SpecimenCriteria criteria, Expression<Func<T, MolecularDataIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.MgmtStatus))
        {
            Add(new BooleanFilter<T>(
                FilterNames.MgmtStatus,
                criteria.MgmtStatus.Not,
                path.Join(molecularData => molecularData.MgmtStatus),
                criteria.MgmtStatus.Value
            ));
        }

        if (IsNotEmpty(criteria.IdhStatus))
        {
            Add(new BooleanFilter<T>(
                FilterNames.IdhStatus,
                criteria.IdhStatus.Not,
                path.Join(molecularData => molecularData.IdhStatus),
                criteria.IdhStatus.Value
            ));
        }

        if (IsNotEmpty(criteria.IdhMutation))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.IdhMutation,
                criteria.IdhMutation.Not,
                path.Join(molecularData => molecularData.IdhMutation.Suffix(_keywordSuffix)),
                criteria.IdhMutation.Value
            ));
        }

        if (IsNotEmpty(criteria.TertStatus))
        {
            Add(new BooleanFilter<T>(
                FilterNames.TertStatus,
                criteria.TertStatus.Not,
                path.Join(molecularData => molecularData.TertStatus),
                criteria.TertStatus.Value
            ));
        }

        if (IsNotEmpty(criteria.TertMutation))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.TertMutation,
                criteria.TertMutation.Not,
                path.Join(molecularData => molecularData.TertMutation.Suffix(_keywordSuffix)),
                criteria.TertMutation.Value
            ));
        }

        if (IsNotEmpty(criteria.ExpressionSubtype))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.ExpressionSubtype,
                criteria.ExpressionSubtype.Not,
                path.Join(molecularData => molecularData.ExpressionSubtype.Suffix(_keywordSuffix)),
                criteria.ExpressionSubtype.Value
            ));
        }

        if (IsNotEmpty(criteria.MethylationSubtype))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.MethylationSubtype,
                criteria.MethylationSubtype.Not,
                path.Join(molecularData => molecularData.MethylationSubtype.Suffix(_keywordSuffix)),
                criteria.MethylationSubtype.Value
            ));
        }

        if (IsNotEmpty(criteria.GcimpMethylation))
        {
            Add(new BooleanFilter<T>(
                FilterNames.GcimpMethylation,
                criteria.GcimpMethylation.Not,
                path.Join(molecularData => molecularData.GcimpMethylation),
                criteria.GcimpMethylation.Value
            ));
        }

        if (IsNotEmpty(criteria.GeneKnockout))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.GeneKnockout,
                criteria.GeneKnockout.Not,
                path.Join(molecularData => molecularData.GeneKnockouts.First().Suffix(_keywordSuffix)),
                criteria.GeneKnockout.Value
            ));
        }
    }
}
