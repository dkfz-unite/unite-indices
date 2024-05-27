namespace Unite.Indices.Entities.Basic.Genome.Dna;

public class AffectedTranscriptIndex
{
    public TranscriptIndex Feature { get; set; }

    public string AminoAcidChange { get; set; }
    public string CodonChange { get; set; }

    public int? Distance { get; set; }
    public int? OverlapBpNumber { get; set; }
    public double? OverlapPercentage { get; set; }
}
