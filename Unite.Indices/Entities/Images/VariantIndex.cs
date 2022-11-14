using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Indices.Entities.Images;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    public AffectedFeatureIndex[] AffectedFeatures { get; set; }
}
