using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;

public record class ProteinsCriteria : CriteriaCollection
{
    public ValuesCriteria<int> Id { get; set; }
    
    public RangeCriteria<double?> Expression { get; set; }

    public BoolCriteria HasSms { get; set; }
    public BoolCriteria HasCnvs { get; set; }
    public BoolCriteria HasSvs { get; set; }
}
