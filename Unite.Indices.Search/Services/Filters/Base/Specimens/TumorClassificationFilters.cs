using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public class TumorClassificationFilters<T> : FiltersCollection<T> where T : class
{
    protected SpecimenFilterNames FilterNames = new();

    public TumorClassificationFilters(SpecimenCriteria criteria, Expression<Func<T, TumorClassificationIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.TumorSuperfamily))
        {
            Add(new SimilarityFilter<T, object>(
                FilterNames.TumorSuperfamily,
                criteria.TumorSuperfamily.Not,
                path.Join(classification => classification.Superfamily.Suffix(_keywordSuffix)),
                criteria.TumorSuperfamily.Value
           ));
        }

        if (IsNotEmpty(criteria.TumorFamily))
        {
            Add(new SimilarityFilter<T, object>(
                FilterNames.TumorFamily,
                criteria.TumorFamily.Not,
                path.Join(classification => classification.Family.Suffix(_keywordSuffix)),
                criteria.TumorFamily.Value
           ));
        }

        if (IsNotEmpty(criteria.TumorClass))
        {
            Add(new SimilarityFilter<T, object>(
                FilterNames.TumorClass,
                criteria.TumorClass.Not,
                path.Join(classification => classification.Class.Suffix(_keywordSuffix)),
                criteria.TumorClass.Value
           ));
        }

        if (IsNotEmpty(criteria.TumorSubClass))
        {
            Add(new SimilarityFilter<T, object>(
                FilterNames.TumorSubclass,
                criteria.TumorSubClass.Not,
                path.Join(classification => classification.Subclass.Suffix(_keywordSuffix)),
                criteria.TumorSubClass.Value
           ));
        }
    }
}
