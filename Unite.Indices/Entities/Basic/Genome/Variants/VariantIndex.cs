namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class VariantIndex
{
    public long Id { get; set; }

    public MutationIndex Mutation { get; set; }
    public CopyNumberVariantIndex CopyNumberVariant { get; set; }
    public StructuralVariantIndex StructuralVariant { get; set; }

    public AffectedTranscriptIndex[] AffectedTranscripts { get; set; }
}
