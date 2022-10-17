namespace Unite.Indices.Entities.Basic.Genome.Variants.SSM;

public class VariantIndex : Variants.VariantIndex
{
    public string Code { get; set; }
    public string Type { get; set; }
    public string Ref { get; set; }
    public string Alt { get; set; }

    public AffectedTranscriptIndex[] AffectedTranscripts { get; set; }
}
