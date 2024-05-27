namespace Unite.Indices.Entities.Basic.Genome.Dna;

public abstract class VariantBaseIndex
{
    public long Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int? Length { get; set; }

    public AffectedFeatureIndex[] AffectedFeatures { get; set; }
}
