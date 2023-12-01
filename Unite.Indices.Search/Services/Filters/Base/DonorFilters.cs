using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Basic.Donors;
using Unite.Essentials.Extensions;

namespace Unite.Indices.Search.Services.Filters.Base;

public class DonorFilters<T> : FiltersCollection<T> where T : class
{
    public DonorFilters(DonorCriteria criteria, Expression<Func<T, DonorIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }

        if (IsNotEmpty(criteria.Id))
        {
            Add(new EqualityFilter<T, int>(
                DonorFilterNames.Id,
                path.Join(donor => donor.Id),
                criteria.Id
            ));
        }

        if (IsNotEmpty(criteria.ReferenceId))
        {
            Add(new SimilarityFilter<T, string>(
                DonorFilterNames.ReferenceId,
                path.Join(donor => donor.ReferenceId),
                criteria.ReferenceId)
            );
        }

        if (IsNotEmpty(criteria.Diagnosis))
        {
            Add(new SimilarityFilter<T, string>(
                DonorFilterNames.Diagnosis,
                path.Join(donor => donor.ClinicalData.Diagnosis),
                criteria.Diagnosis)
            );
        }

        if (IsNotEmpty(criteria.PrimarySite))
        {
            Add(new SimilarityFilter<T, string>(
                DonorFilterNames.PrimarySite,
                path.Join(donor => donor.ClinicalData.PrimarySite),
                criteria.PrimarySite)
            );
        }

        if (IsNotEmpty(criteria.Localization))
        {
            Add(new SimilarityFilter<T, string>(
                DonorFilterNames.Localization,
                path.Join(donor => donor.ClinicalData.Localization),
                criteria.Localization)
            );
        }

        if (IsNotEmpty(criteria.Gender))
        {
            Add(new EqualityFilter<T, object>(
                DonorFilterNames.Gender,
                path.Join(donor => donor.ClinicalData.Gender.Suffix(_keywordSuffix)),
                criteria.Gender)
            );
        }

        if (IsNotEmpty(criteria.Age))
        {
            Add(new RangeFilter<T, int?>(
                DonorFilterNames.Age,
                path.Join(donor => donor.ClinicalData.Age),
                criteria.Age?.From,
                criteria.Age?.To)
            );
        }

        if (IsNotEmpty(criteria.VitalStatus))
        {
            Add(new BooleanFilter<T>(
                DonorFilterNames.VitalStatus,
                path.Join(donor => donor.ClinicalData.VitalStatus),
                criteria.VitalStatus)
            );
        }

        if (IsNotEmpty(criteria.VitalStatusChangeDay))
        {
            Add(new RangeFilter<T, int?>(
                DonorFilterNames.VitalStatusChangeDay,
                path.Join(donor => donor.ClinicalData.VitalStatusChangeDay),
                criteria.VitalStatusChangeDay?.From,
                criteria.VitalStatusChangeDay?.To)
            );
        }

        if (IsNotEmpty(criteria.ProgressionStatus))
        {
            Add(new BooleanFilter<T>(
                DonorFilterNames.ProgressionStatus,
                path.Join(donor => donor.ClinicalData.ProgressionStatus),
                criteria.ProgressionStatus)
            );
        }

        if (IsNotEmpty(criteria.ProgressionStatusChangeDay))
        {
            Add(new RangeFilter<T, int?>(
                DonorFilterNames.ProgressionStatusChangeDay,
                path.Join(donor => donor.ClinicalData.ProgressionStatusChangeDay),
                criteria.ProgressionStatusChangeDay?.From,
                criteria.ProgressionStatusChangeDay?.To)
            );
        }

        if (IsNotEmpty(criteria.Therapy))
        {
            Add(new SimilarityFilter<T, object>(
               DonorFilterNames.Therapy,
               path.Join(donor => donor.Treatments.First().Therapy.Suffix(_keywordSuffix)),
               criteria.Therapy)
           );
        }

        if (IsNotEmpty(criteria.MtaProtected))
        {
            Add(new BooleanFilter<T>(
                DonorFilterNames.MtaProtected,
                path.Join(donor => donor.MtaProtected),
                criteria.MtaProtected)
            );
        }

        if (IsNotEmpty(criteria.Project))
        {
            Add(new SimilarityFilter<T, object>(
               DonorFilterNames.Project,
               path.Join(donor => donor.Projects.First().Name.Suffix(_keywordSuffix)),
               criteria.Project)
           );
        }

        if (IsNotEmpty(criteria.Study))
        {
            Add(new SimilarityFilter<T, object>(
               DonorFilterNames.Study,
               path.Join(donor => donor.Studies.First().Name.Suffix(_keywordSuffix)),
               criteria.Study)
           );
        }
    }
}
