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
                path.Join(donor => donor.ReferenceId),
                criteria.ReferenceId.Value
            ));
        }

        if (IsNotEmpty(criteria.Diagnosis))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Diagnosis,
                path.Join(donor => donor.ClinicalData.Diagnosis),
                criteria.Diagnosis.Value
            ));
        }

        if (IsNotEmpty(criteria.PrimarySite))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.PrimarySite,
                path.Join(donor => donor.ClinicalData.PrimarySite),
                criteria.PrimarySite.Value
            ));
        }

        if (IsNotEmpty(criteria.Localization))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Localization,
                path.Join(donor => donor.ClinicalData.Localization),
                criteria.Localization.Value
            ));
        }

        if (IsNotEmpty(criteria.Sex))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Sex,
                path.Join(donor => donor.ClinicalData.Sex.Suffix(_keywordSuffix)),
                criteria.Sex.Value
            ));
        }

        if (IsNotEmpty(criteria.Age))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.Age,
                path.Join(donor => donor.ClinicalData.Age),
                criteria.Age.Value?.From,
                criteria.Age.Value?.To
            ));
        }

        if (IsNotEmpty(criteria.VitalStatus))
        {
            Add(new BooleanFilter<T>(
                FilterNames.VitalStatus,
                path.Join(donor => donor.ClinicalData.VitalStatus),
                criteria.VitalStatus.Value
            ));
        }

        if (IsNotEmpty(criteria.VitalStatusChangeDay))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.VitalStatusChangeDay,
                path.Join(donor => donor.ClinicalData.VitalStatusChangeDay),
                criteria.VitalStatusChangeDay?.Value.From,
                criteria.VitalStatusChangeDay?.Value.To
            ));
        }

        if (IsNotEmpty(criteria.ProgressionStatus))
        {
            Add(new BooleanFilter<T>(
                FilterNames.ProgressionStatus,
                path.Join(donor => donor.ClinicalData.ProgressionStatus),
                criteria.ProgressionStatus.Value
            ));
        }

        if (IsNotEmpty(criteria.ProgressionStatusChangeDay))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.ProgressionStatusChangeDay,
                path.Join(donor => donor.ClinicalData.ProgressionStatusChangeDay),
                criteria.ProgressionStatusChangeDay?.Value.From,
                criteria.ProgressionStatusChangeDay?.Value.To
            ));
        }

        if (IsNotEmpty(criteria.Therapy))
        {
            Add(new SimilarityFilter<T, object>(
               FilterNames.Therapy,
               path.Join(donor => donor.Treatments.First().Therapy.Suffix(_keywordSuffix)),
               criteria.Therapy.Value
           ));
        }

        if (IsNotEmpty(criteria.MtaProtected))
        {
            Add(new BooleanFilter<T>(
                FilterNames.MtaProtected,
                path.Join(donor => donor.MtaProtected),
                criteria.MtaProtected.Value
            ));
        }

        if (IsNotEmpty(criteria.Project))
        {
            Add(new SimilarityFilter<T, object>(
               FilterNames.Project,
               path.Join(donor => donor.Projects.First().Name.Suffix(_keywordSuffix)),
               criteria.Project.Value
           ));
        }

        if (IsNotEmpty(criteria.Study))
        {
            Add(new SimilarityFilter<T, object>(
               FilterNames.Study,
               path.Join(donor => donor.Studies.First().Name.Suffix(_keywordSuffix)),
               criteria.Study.Value
           ));
        }
    }
}
