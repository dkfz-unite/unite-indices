namespace Unite.Indices.Entities.Basic.Omics;

public class GeneIndex : GeneNavIndex
{
    public string StableId { get; set; }
    [Searchable]
    public string Symbol { get; set; }
    public string Description { get; set; }
    [Searchable]
    public string Biotype { get; set; }
    [Searchable]
    public string Chromosome { get; set; }
    public string ChromosomeArm { get; set; }
    [Searchable]
    public int? Start { get; set; }
    [Searchable]
    public int? End { get; set; }
    public bool? Strand { get; set; }
    public int? ExonicLength { get; set; }
}
