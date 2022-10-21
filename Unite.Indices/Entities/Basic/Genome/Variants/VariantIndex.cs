namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class VariantIndex
{
    public string Id => GetId();

    public MutationIndex Mutation { get; set; }
    public CopyNumberVariantIndex CopyNumberVariant { get; set; }
    public StructuralVariantIndex StructuralVariant { get; set; }

    public AffectedTranscriptIndex[] AffectedTranscripts { get; set; }


    private string GetId()
    {
        if (Mutation != null)
        {
            return $"SSM{Mutation.Id}";
        }
        else if (CopyNumberVariant != null)
        {
            return $"CNV{CopyNumberVariant.Id}";
        }
        else if (StructuralVariant != null)
        {
            return $"SV{StructuralVariant.Id}";
        }
        else
        {
            throw new NotSupportedException("Variant type is not recognized");
        }
    }
}
