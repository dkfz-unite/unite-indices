namespace Unite.Indices.Entities.Basic.Genome.Variants;

public abstract class VariantBaseIndex : VariantNavIndex
{
    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public int? Length { get; set; }

    public AffectedFeatureIndex[] AffectedFeatures { get; set; }
}
