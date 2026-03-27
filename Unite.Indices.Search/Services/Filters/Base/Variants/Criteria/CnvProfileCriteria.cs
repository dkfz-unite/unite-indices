using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record CnvProfileCriteria: CriteriaCollection
{
    public ValuesCriteria<int> SpecimenId { get; set; }
    public ValuesCriteria<string> Chromosome { get; set; }
    public ValuesCriteria<string> ChromosomeArm { get; set; }
    public RangeCriteria<float?> Gain { get; set; }
    public RangeCriteria<float?> Loss { get; set; }
    public RangeCriteria<float?> Neutral { get; set; }
}