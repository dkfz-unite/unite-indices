namespace Unite.Indices.Entities.Basic.Genome.Variants;

public abstract class AffectedFeatureIndex<TFeatureIndex> where TFeatureIndex : FeatureIndex
{
    public TFeatureIndex Feature { get; set; }

    public GeneIndex Gene { get; set; }

    public ConsequenceIndex[] Consequences { get; set; }
}
