using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;

public record ProteinCriteria : ProteinsCriteria
{
    public ValuesCriteria<string> Symbol { get; set; }
    public ValuesCriteria<string> Accession { get; set; }
    public ValuesCriteria<string> Chromosome { get; set; }
    public RangeCriteria<double?> Position { get; set; }
}
