namespace Unite.Indices.Entities.Basic.Genome.Dna;

public class AffectedTranscriptIndex
{
    public TranscriptIndex Feature { get; set; }

    public string ProteinChange { get; set; }
    public string CodonChange { get; set; }

    public int? Distance { get; set; }
    public int? OverlapBpNumber { get; set; }
    public double? OverlapPercentage { get; set; }
}
