namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class CopyNumberVariantIndex
{
    public long Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int Length { get; set; }

    public string Type { get; set; }
    public bool? Loh { get; set; }
    public bool? HomoDel { get; set; }
    public double? C1Mean { get; set; }
    public double? C2Mean { get; set; }
    public double? TcnMean { get; set; }
    public int? C1 { get; set; }
    public int? C2 { get; set; }
    public int? Tcn { get; set; }
    public double? TcnRatio { get; set; }
    public double? DhMax { get; set; }

    public AffectedFeatureIndex[] AffectedFeatures { get; set; }
}
