namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class AffectedTranscriptIndex : AffectedFeatureIndex<TranscriptIndex>
{
    public string AminoAcidChange { get; set; }
    public string CodonChange { get; set; }

    public int? Distance { get; set; }
    public int? OverlapBpNumber { get; set; }
    public int? OverlapPercentage { get; set; }
}
