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
            Add(new EqualityFilter<T, object>(
                FilterNames.MgmtStatus,
                path.Join(molecularData => molecularData.MgmtStatus.Suffix(_keywordSuffix)),
                criteria.MgmtStatus
            ));
        }

        if (IsNotEmpty(criteria.IdhStatus))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.IdhStatus,
                path.Join(molecularData => molecularData.IdhStatus.Suffix(_keywordSuffix)),
                criteria.IdhStatus
            ));
        }

        if (IsNotEmpty(criteria.IdhMutation))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.IdhMutation,
                path.Join(molecularData => molecularData.IdhMutation.Suffix(_keywordSuffix)),
                criteria.IdhMutation
            ));
        }

        if (IsNotEmpty(criteria.GeneExpressionSubtype))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.GeneExpressionSubtype,
                path.Join(molecularData => molecularData.GeneExpressionSubtype.Suffix(_keywordSuffix)),
                criteria.GeneExpressionSubtype
            ));
        }

        if (IsNotEmpty(criteria.MethylationSubtype))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.MethylationSubtype,
                path.Join(molecularData => molecularData.MethylationSubtype.Suffix(_keywordSuffix)),
                criteria.MethylationSubtype
            ));
        }

        if (IsNotEmpty(criteria.GcimpMethylation))
        {
            Add(new BooleanFilter<T>(
                FilterNames.GcimpMethylation,
                path.Join(molecularData => molecularData.GcimpMethylation),
                criteria.GcimpMethylation
            ));
        }
    }
}
