namespace Unite.Indices.Entities.Basic.Omics;

public class ProteinIndex : ProteinNavIndex
{
    public string StableId { get; set; }
    public string AccessionId { get; set; }
    public string Database { get; set; }
    public string Symbol { get; set; }
    public string Description { get; set; }
    public bool? IsCanonical { get; set; }
    public string Chromosome { get; set; }
    public int? Start { get; set; }
    public int? End { get; set; }
    public int? Length { get; set; }
}
