namespace Unite.Indices.Entities.Basic.Genome.Variants.CNV;

public class AffectedTranscriptIndex : AffectedFeatureIndex<TranscriptIndex>
{
    public int? Distance { get; set; }
    public int? OverlapBpNumber { get; set; }
    public int? OverlapPercentage { get; set; }
}
