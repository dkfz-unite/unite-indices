using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record CnvProfileCriteria: CriteriaCollection
{
    public ValuesCriteria<string> Chromosome { get; set; }
    public ValuesCriteria<string> ChromosomeArm { get; set; }
}