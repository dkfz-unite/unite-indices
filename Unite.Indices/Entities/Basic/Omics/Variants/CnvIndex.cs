namespace Unite.Indices.Entities.Basic.Omics.Variants;

public class CnvIndex : VariantBaseIndex
{
    [Searchable]
    public string Type { get; set; }
    [Searchable]
    public bool? Loh { get; set; }
    [Searchable]
    public bool? Del { get; set; }
    public double? C1Mean { get; set; }
    public double? C2Mean { get; set; }
    public double? TcnMean { get; set; }
    public int? C1 { get; set; }
    public int? C2 { get; set; }
    public int? Tcn { get; set; }
    public double? TcnRatio { get; set; }
}
