namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class AffectedFeatureIndex
{
    public GeneIndex Gene { get; set; }

    public AffectedTranscriptIndex Transcript { get; set; }
    //public AffectedRegulatorIndex Regulator { get; set; }
    //public AffectedMotifIndex Motif { get; set; }

    public ConsequenceIndex[] Consequences { get; set; }
}
