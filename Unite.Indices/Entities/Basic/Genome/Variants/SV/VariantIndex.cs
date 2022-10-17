namespace Unite.Indices.Entities.Basic.Genome.Variants.SV;

public class VariantIndex : Variants.VariantIndex
{
    public string NewChromosome { get; set; }
    public double? NewStart { get; set; }
    public double? NewEnd { get; set; }
    public string Type { get; set; }
    public string Ref { get; set; }
    public string Alt { get; set; }

    public AffectedTranscriptIndex[] AffectedTranscripts { get; set; }
}
