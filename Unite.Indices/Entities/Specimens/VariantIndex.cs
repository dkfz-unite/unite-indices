using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Indices.Entities.Specimens;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    public AffectedFeatureIndex[] AffectedFeatures { get; set; }
}
