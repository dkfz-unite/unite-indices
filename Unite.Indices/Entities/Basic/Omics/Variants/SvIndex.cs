namespace Unite.Indices.Entities.Basic.Omics.Variants;

public class SvIndex : VariantBaseIndex
{
    public string OtherChromosome { get; set; }
    public string OtherChromosomeArm { get; set; }
    public int OtherChromosomeI => GetIndex(OtherChromosome);
    [Searchable]
    public int OtherStart { get; set; }
    public int OtherEnd { get; set; }

    [Searchable]
    public string Type { get; set; }
    [Searchable]
    public bool? Inverted { get; set; }
}
