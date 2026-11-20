namespace Unite.Indices.Entities.Basic.Omics.Variants;

public class AffectedFeatureIndex
{
    [Searchable]
    [References(typeof(Genes.GeneIndex))]
    public GeneIndex Gene { get; set; }

    [Searchable]
    public AffectedTranscriptIndex Transcript { get; set; }
    //public AffectedRegulatorIndex Regulator { get; set; }
    //public AffectedMotifIndex Motif { get; set; }

    [Searchable]
    public EffectIndex[] Effects { get; set; }
}
