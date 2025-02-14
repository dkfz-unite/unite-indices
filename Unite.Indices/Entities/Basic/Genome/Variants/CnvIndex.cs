namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class CnvIndex : VariantBaseIndex
{
    public string Type { get; set; }
    public bool? Loh { get; set; }
    public bool? Del { get; set; }
    public double? C1Mean { get; set; }
    public double? C2Mean { get; set; }
    public double? TcnMean { get; set; }
    public int? C1 { get; set; }
    public int? C2 { get; set; }
    public int? Tcn { get; set; }
    public double? TcnRatio { get; set; }
}
