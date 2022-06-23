namespace Unite.Indices.Entities.Basic.Genome.Mutations;

public class AffectedTranscriptIndex
{
    public long Id { get; set; }

    public string AminoAcidChange { get; set; }
    public string CodonChange { get; set; }
    public ConsequenceIndex[] Consequences { get; set; }

    public TranscriptIndex Transcript { get; set; }
}
