using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

public record GeneCriteria : GenesCriteria
{
    public ValuesCriteria<string> Symbol { get; set; }
    public ValuesCriteria<string> Biotype { get; set; }
    public ValuesCriteria<string> Chromosome { get; set; }
    public RangeCriteria<double?> Position { get; set; }


    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Symbol?.IsNotEmpty() == true
            || Biotype?.IsNotEmpty() == true
            || Chromosome?.IsNotEmpty() == true
            || Position?.IsNotEmpty() == true;
    }
}
