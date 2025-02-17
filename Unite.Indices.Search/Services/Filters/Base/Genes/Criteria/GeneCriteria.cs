using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

public record GeneCriteria : GenesCriteria
{
    public string[] Symbol { get; set; }
    public string[] Biotype { get; set; }
    public string[] Chromosome { get; set; }
    public Range<double?> Position { get; set; }


    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Symbol?.Length > 0
            || Biotype?.Length > 0
            || Chromosome?.Length > 0
            || Position?.From != null
            || Position?.To != null;
    }
}
