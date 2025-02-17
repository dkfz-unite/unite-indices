namespace Unite.Indices.Entities.Basic.Genome;

public class GeneIndex : GeneNavIndex
{
    public string StableId { get; set; }
    public string Symbol { get; set; }
    public string Description { get; set; }
    public string Biotype { get; set; }
    public string Chromosome { get; set; }
    public int? Start { get; set; }
    public int? End { get; set; }
    public bool? Strand { get; set; }
    public int? ExonicLength { get; set; }
}
