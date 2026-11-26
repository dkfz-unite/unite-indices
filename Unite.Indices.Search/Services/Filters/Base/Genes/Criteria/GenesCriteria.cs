using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

public record GenesCriteria : GenesDataCriteria
{
    public ValuesCriteria<int> Id { get; set; }

    public RangeCriteria<double?> Expression { get; set; }
}
