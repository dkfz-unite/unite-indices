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
                criteria.ReferenceId
            ));
        }

        if (IsNotEmpty(criteria.Diagnosis))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Diagnosis,
                path.Join(donor => donor.ClinicalData.Diagnosis),
                criteria.Diagnosis
            ));
        }

        if (IsNotEmpty(criteria.PrimarySite))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.PrimarySite,
                path.Join(donor => donor.ClinicalData.PrimarySite),
                criteria.PrimarySite
            ));
        }

        if (IsNotEmpty(criteria.Localization))
        {
            Add(new SimilarityFilter<T, string>(
                FilterNames.Localization,
                path.Join(donor => donor.ClinicalData.Localization),
                criteria.Localization
            ));
        }

        if (IsNotEmpty(criteria.Sex))
        {
            Add(new EqualityFilter<T, object>(
                FilterNames.Sex,
                path.Join(donor => donor.ClinicalData.Sex.Suffix(_keywordSuffix)),
                criteria.Sex
            ));
        }

        if (IsNotEmpty(criteria.Age))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.Age,
                path.Join(donor => donor.ClinicalData.Age),
                criteria.Age?.From,
                criteria.Age?.To
            ));
        }

        if (IsNotEmpty(criteria.VitalStatus))
        {
            Add(new BooleanFilter<T>(
                FilterNames.VitalStatus,
                path.Join(donor => donor.ClinicalData.VitalStatus),
                criteria.VitalStatus
            ));
        }

        if (IsNotEmpty(criteria.VitalStatusChangeDay))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.VitalStatusChangeDay,
                path.Join(donor => donor.ClinicalData.VitalStatusChangeDay),
                criteria.VitalStatusChangeDay?.From,
                criteria.VitalStatusChangeDay?.To
            ));
        }

        if (IsNotEmpty(criteria.ProgressionStatus))
        {
            Add(new BooleanFilter<T>(
                FilterNames.ProgressionStatus,
                path.Join(donor => donor.ClinicalData.ProgressionStatus),
                criteria.ProgressionStatus
            ));
        }

        if (IsNotEmpty(criteria.ProgressionStatusChangeDay))
        {
            Add(new RangeFilter<T, int?>(
                FilterNames.ProgressionStatusChangeDay,
                path.Join(donor => donor.ClinicalData.ProgressionStatusChangeDay),
                criteria.ProgressionStatusChangeDay?.From,
                criteria.ProgressionStatusChangeDay?.To
            ));
        }

        if (IsNotEmpty(criteria.Therapy))
        {
            Add(new SimilarityFilter<T, object>(
               FilterNames.Therapy,
               path.Join(donor => donor.Treatments.First().Therapy.Suffix(_keywordSuffix)),
               criteria.Therapy
           ));
        }

        if (IsNotEmpty(criteria.MtaProtected))
        {
            Add(new BooleanFilter<T>(
                FilterNames.MtaProtected,
                path.Join(donor => donor.MtaProtected),
                criteria.MtaProtected
            ));
        }

        if (IsNotEmpty(criteria.Project))
        {
            Add(new SimilarityFilter<T, object>(
               FilterNames.Project,
               path.Join(donor => donor.Projects.First().Name.Suffix(_keywordSuffix)),
               criteria.Project
           ));
        }

        if (IsNotEmpty(criteria.Study))
        {
            Add(new SimilarityFilter<T, object>(
               FilterNames.Study,
               path.Join(donor => donor.Studies.First().Name.Suffix(_keywordSuffix)),
               criteria.Study
           ));
        }
    }
}
