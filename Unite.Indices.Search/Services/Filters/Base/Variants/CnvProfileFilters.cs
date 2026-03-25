using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.CnvProfiles;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Constants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants;

public class CnvProfileFilters<TContainerEntity> : FiltersCollection<TContainerEntity> 
    where TContainerEntity : class
{
    public CnvProfileFilters(CnvProfileCriteria criteria, Expression<Func<TContainerEntity, CnvProfileIndex>> path)
    {
        if (criteria == null)
        {
            return;
        }
        
        //TODO: the Add method could be significantly simplified(criteria property name is mentioned 3(!) times and can easily lead to errors if other property of the same time will be mentioned in one of the places)
        if (IsNotEmpty(criteria.SpecimenId))
        {
            Add(new EqualityFilter<TContainerEntity, int>(
                CnvProfileFilterNames.Specimen,
                criteria.SpecimenId.Not,
                path.Join(cnvProfile => cnvProfile.Specimen.Id),
                criteria.SpecimenId.Value
            ));
        }
        
        if (IsNotEmpty(criteria.Chromosome))
        {
            Add(new SimilarityFilter<TContainerEntity, string>(
                CnvProfileFilterNames.Chromosome,
                criteria.Chromosome.Not,
                path.Join(cnvProfile => cnvProfile.Chromosome),
                criteria.Chromosome.Value
            ));
        }
        
        if (IsNotEmpty(criteria.ChromosomeArm))
        {
            Add(new SimilarityFilter<TContainerEntity, string>(
                CnvProfileFilterNames.ChromosomeArm,
                criteria.ChromosomeArm.Not,
                path.Join(cnvProfile => cnvProfile.ChromosomeArm),
                criteria.ChromosomeArm.Value
            ));
        }
        
        if (IsNotEmpty(criteria.Gain))
        {
            Add(new RangeFilter<TContainerEntity, float>(
                CnvProfileFilterNames.Gain,
                criteria.Gain.Not,
                path.Join(cnvProfile => cnvProfile.Gain),
                criteria.Gain.Value?.From,
                criteria.Gain.Value?.To
            ));
        }
        
        if (IsNotEmpty(criteria.Loss))
        {
            Add(new RangeFilter<TContainerEntity, float>(
                CnvProfileFilterNames.Loss,
                criteria.Loss.Not,
                path.Join(cnvProfile => cnvProfile.Loss),
                criteria.Loss.Value?.From,
                criteria.Loss.Value?.To
            ));
        }
        
        if (IsNotEmpty(criteria.Neutral))
        {
            Add(new RangeFilter<TContainerEntity, float>(
                CnvProfileFilterNames.Neutral,
                criteria.Neutral.Not,
                path.Join(cnvProfile => cnvProfile.Neutral),
                criteria.Neutral.Value?.From,
                criteria.Neutral.Value?.To
            ));
        }
    }
}