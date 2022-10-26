namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class StructuralVariantIndex
{
    public long Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }

    public string NewChromosome { get; set; }
    public double? NewStart { get; set; }
    public double? NewEnd { get; set; }

    public string Type { get; set; }
    public string Ref { get; set; }
    public string Alt { get; set; }
}
