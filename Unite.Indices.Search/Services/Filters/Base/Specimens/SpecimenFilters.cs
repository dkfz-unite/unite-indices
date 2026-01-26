using System.Linq.Expressions;
using Nest;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Specimens;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens;

public abstract class SpecimenFilters<T, TModel> : FiltersCollection<T> 
    where T : class
    where TModel : SpecimenBaseIndex
{
    protected abstract SpecimenFilterNames FilterNames { get; }

    protected virtual bool IncludeTumorClassification => true;
    protected virtual bool IncludeMolecularData => true;
    protected virtual bool IncludeInterventions => true;
    protected virtual bool IncludeDrugScreenings => true;


    public SpecimenFilters(SpecimenCriteria criteria, Expression<Func<T, TModel>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                FilterNames.Id,
                criteria.Id.Not,
                path.Join(specimen => specimen.Id),
                criteria.Id.Value
            ));
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.ReferenceId,
                criteria.ReferenceId.Not,
                path.Join(specimen => specimen.ReferenceId),
                criteria.ReferenceId.Value
            ));
        }

        if (IsNotEmpty(criteria.Category))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Category,
                criteria.Category.Not,
                path.Join(specimen => specimen.Category.Suffix(_keywordSuffix)),
                criteria.Category.Value
            ));
        }

        if (IsNotEmpty(criteria.TumorType))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.TumorType,
                criteria.TumorType.Not,
                path.Join(specimen => specimen.TumorType.Suffix(_keywordSuffix)),
                criteria.TumorType.Value
            ));
        }

        if (IsNotEmpty(criteria.TumorGrade))
        {
            Add(new RangeFilter<T, double?>(
                FilterNames.TumorGrade,
                criteria.TumorGrade.Not,
                path.Join(specimen => specimen.TumorGrade),
                criteria.TumorGrade.Value?.From,
                criteria.TumorGrade.Value?.To
            ));
        }

        
        if (IncludeTumorClassification)
        {
            var tumorClassificationFilters = new TumorClassificationFilters<T>(criteria, path.Join(specimen => specimen.TumorClassification));
        
            Add(tumorClassificationFilters.All());
        }

        if (IncludeMolecularData)
        {
            var molecularDataFilters = new MolecularDataFilters<T>(criteria, path.Join(specimen => specimen.MolecularData));

            Add(molecularDataFilters.All());
        }

        if (IncludeInterventions)
        {
            var interventionFilters = new InterventionFilters<T>(criteria, path.Join(specimen => specimen.Interventions));

            Add(interventionFilters.All());
        }

        if (IncludeDrugScreenings)
        {
            var drugScreeningFilters = new DrugScreeningFilters<T>(criteria, path.Join(specimen => specimen.DrugScreenings));

            Add(drugScreeningFilters.All());
        }
    }
}
