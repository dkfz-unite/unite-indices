namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class VariantIndex
{
    private string _id;


    public string Id
    {
        get => _id ?? GetId();
        set => _id = value;
    }

    public MutationIndex Mutation { get; set; }
    public CopyNumberVariantIndex CopyNumberVariant { get; set; }
    public StructuralVariantIndex StructuralVariant { get; set; }


    public AffectedFeatureIndex[] GetAffectedFeatures()
    {
        return Mutation != null ? Mutation.AffectedFeatures :
               CopyNumberVariant != null ? CopyNumberVariant.AffectedFeatures :
               StructuralVariant != null ? StructuralVariant.AffectedFeatures :
               throw new NullReferenceException("Specific variant is not set");
    }

    private string GetId()
    {
        return Mutation != null ? $"SSM{Mutation.Id}" :
               CopyNumberVariant != null ? $"CNV{CopyNumberVariant.Id}" :
               StructuralVariant != null ? $"SV{StructuralVariant.Id}" :
               throw new NullReferenceException("Specific variant is not set");
    }
}
