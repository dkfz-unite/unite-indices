using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Indices.Entities.Genes;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    public ConsequenceIndex[] Consequences { get; set; }
}
