namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class SvIndex
{
    public long Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }

    public string OtherChromosome { get; set; }
    public int OtherStart { get; set; }
    public int OtherEnd { get; set; }

    public int? Length { get; set; }

    public string Type { get; set; }
    public bool? Inverted { get; set; }

    public AffectedFeatureIndex[] AffectedFeatures { get; set; }
}
