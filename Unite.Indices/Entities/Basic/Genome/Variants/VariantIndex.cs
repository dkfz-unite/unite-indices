namespace Unite.Indices.Entities.Basic.Genome.Variants;

public abstract class VariantIndex
{
    public long Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
}
