using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Indices.Entities.Donors;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    public AffectedFeatureIndex[] AffectedFeatures { get; set; }
}
