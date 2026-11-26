using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;

public record class ProteinsCriteria : ProteinsDataCriteria
{
    public ValuesCriteria<int> Id { get; set; }
    
    public RangeCriteria<double?> Expression { get; set; }
}
