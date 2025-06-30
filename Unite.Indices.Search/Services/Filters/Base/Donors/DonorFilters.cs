using Nest;
using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Donors;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Constants;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Donors;

public class DonorFilters<T> : FiltersCollection<T> where T : class
{
    protected DonorFilterNames FilterNames = new();

    public DonorFilters(DonorCriteria criteria, Expression<Func<T, DonorIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.ReferenceId,
                criteria.ReferenceId.Not,
                path.Join(donor => donor.ReferenceId),
                criteria.ReferenceId.Value
            ));
        }

        if (IsNotEmpty(criteria.Diagnosis))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Diagnosis,
                criteria.Diagnosis.Not,
                path.Join(donor => donor.ClinicalData.Diagnosis),
                criteria.Diagnosis.Value
            ));
        }

        if (IsNotEmpty(criteria.PrimarySite))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.PrimarySite,
                criteria.PrimarySite.Not,
                path.Join(donor => donor.ClinicalData.PrimarySite),
                criteria.PrimarySite.Value
            ));
        }

        if (IsNotEmpty(criteria.Localization))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Localization,
                criteria.Localization.Not,
                path.Join(donor => donor.ClinicalData.Localization),
                criteria.Localization.Value
            ));
        }

        if (IsNotEmpty(criteria.Sex))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Sex,
                criteria.Sex.Not,
                path.Join(donor => donor.ClinicalData.Sex.Suffix(_keywordSuffix)),
                criteria.Sex.Value
            ));
        }

        if (IsNotEmpty(criteria.Age))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.Age,
                criteria.Age.Not,
                path.Join(donor => donor.ClinicalData.Age),
                criteria.Age.Value?.From,
                criteria.Age.Value?.To
            ));
        }

        if (IsNotEmpty(criteria.VitalStatus))
        {
            Add(new BooleanFilter<T>(
                FilterNames.VitalStatus,
                criteria.VitalStatus.Not,
                path.Join(donor => donor.ClinicalData.VitalStatus),
                criteria.VitalStatus.Value
            ));
        }

        if (IsNotEmpty(criteria.VitalStatusChangeDay))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.VitalStatusChangeDay,
                criteria.VitalStatusChangeDay.Not,
                path.Join(donor => donor.ClinicalData.VitalStatusChangeDay),
                criteria.VitalStatusChangeDay?.Value.From,
                criteria.VitalStatusChangeDay?.Value.To
            ));
        }

        if (IsNotEmpty(criteria.ProgressionStatus))
        {
            Add(new BooleanFilter<T>(
                FilterNames.ProgressionStatus,
                criteria.ProgressionStatus.Not,
                path.Join(donor => donor.ClinicalData.ProgressionStatus),
                criteria.ProgressionStatus.Value
            ));
        }

        if (IsNotEmpty(criteria.ProgressionStatusChangeDay))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.ProgressionStatusChangeDay,
                criteria.ProgressionStatusChangeDay.Not,
                path.Join(donor => donor.ClinicalData.ProgressionStatusChangeDay),
                criteria.ProgressionStatusChangeDay?.Value.From,
                criteria.ProgressionStatusChangeDay?.Value.To
            ));
        }

        if (IsNotEmpty(criteria.Therapy))
        {
            Add(new SimilarityFilter<T, object>(
               FilterNames.Therapy,
               criteria.Therapy.Not,
               path.Join(donor => donor.Treatments.First().Therapy.Suffix(_keywordSuffix)),
               criteria.Therapy.Value
           ));
        }

        if (IsNotEmpty(criteria.MtaProtected))
        {
            Add(new BooleanFilter<T>(
                FilterNames.MtaProtected,
                criteria.MtaProtected.Not,
                path.Join(donor => donor.MtaProtected),
                criteria.MtaProtected.Value
            ));
        }

        if (IsNotEmpty(criteria.Project))
        {
            Add(new SimilarityFilter<T, object>(
                FilterNames.Project,
                criteria.Project.Not,
                path.Join(donor => donor.Projects.First().Name.Suffix(_keywordSuffix)),
                criteria.Project.Value
           ));
        }

        if (IsNotEmpty(criteria.Study))
        {
            Add(new SimilarityFilter<T, object>(
                FilterNames.Study,
                criteria.Study.Not,
                path.Join(donor => donor.Studies.First().Name.Suffix(_keywordSuffix)),
                criteria.Study.Value
           ));
        }
    }
}
