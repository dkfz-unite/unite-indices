namespace Unite.Indices.Entities.Basic.Genome.Variants.CNV;

public class VariantIndex : Variants.VariantIndex
{
    public string SvType { get; set; }
    public string CnaType { get; set; }
    public bool? Loh { get; set; }
    public bool? HomoDel { get; set; }
    public double C1Mean { get; set; }
    public double C2Mean { get; set; }
    public double? TcnMean { get; set; }
    public int? C1 { get; set; }
    public int? C2 { get; set; }
    public int? Tcn { get; set; }
    public double? DhMax { get; set; }

    public AffectedTranscriptIndex[] AffectedTranscripts { get; set; }
}
