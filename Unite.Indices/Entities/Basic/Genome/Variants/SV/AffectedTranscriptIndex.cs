namespace Unite.Indices.Entities.Basic.Genome.Variants.SV;

public class AffectedTranscriptIndex : AffectedFeatureIndex<TranscriptIndex>
{
    public int? Distance { get; set; }
    public int? OverlapBpNumber { get; set; }
    public int? OverlapPercentage { get; set; }
}
