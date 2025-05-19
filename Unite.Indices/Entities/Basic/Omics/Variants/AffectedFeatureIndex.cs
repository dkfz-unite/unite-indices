namespace Unite.Indices.Entities.Basic.Omics.Variants;

public class AffectedFeatureIndex
{
    public GeneIndex Gene { get; set; }

    public AffectedTranscriptIndex Transcript { get; set; }
    //public AffectedRegulatorIndex Regulator { get; set; }
    //public AffectedMotifIndex Motif { get; set; }

    public EffectIndex[] Effects { get; set; }
}
