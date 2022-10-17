namespace Unite.Indices.Entities.Basic.Genome.Variants.SSM;

public class AffectedTranscriptIndex : AffectedFeatureIndex<TranscriptIndex>
{
    public string AminoAcidChange { get; set; }
    public string CodonChange { get; set; }
}
