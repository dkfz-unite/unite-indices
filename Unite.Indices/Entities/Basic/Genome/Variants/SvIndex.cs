namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class SvIndex : VariantBaseIndex
{
    public string OtherChromosome { get; set; }
    public int OtherChromosomeI => GetIndex(OtherChromosome);
    public int OtherStart { get; set; }
    public int OtherEnd { get; set; }

    public string Type { get; set; }
    public bool? Inverted { get; set; }
}
