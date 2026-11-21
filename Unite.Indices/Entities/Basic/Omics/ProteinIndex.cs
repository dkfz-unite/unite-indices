namespace Unite.Indices.Entities.Basic.Omics;

public class ProteinIndex : ProteinNavIndex
{
    public string StableId { get; set; }
    [Searchable]
    public string AccessionId { get; set; }
    public string Database { get; set; }
    [Searchable]
    public string Symbol { get; set; }
    public string Description { get; set; }
    public bool? IsCanonical { get; set; }
    [Searchable]
    public string Chromosome { get; set; }
    public string ChromosomeArm { get; set; }
    [Searchable]
    public int? Start { get; set; }
    [Searchable]
    public int? End { get; set; }
    public bool? Strand { get; set; }
    public int? Length { get; set; }
}
