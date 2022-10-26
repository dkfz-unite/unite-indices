namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class VariantIndex
{
    public string Id => GetId();

    public string Chromosome => GetChromosome();
    public int Start => GetStart();
    public int End => GetEnd();

    public MutationIndex Mutation { get; set; }
    public CopyNumberVariantIndex CopyNumberVariant { get; set; }
    public StructuralVariantIndex StructuralVariant { get; set; }

    public AffectedFeatureIndex[] AffectedFeatures { get; set; }


    private string GetId()
    {
        return Mutation != null ? $"SSM{Mutation.Id}" :
               CopyNumberVariant != null ? $"CNV{CopyNumberVariant.Id}" :
               StructuralVariant != null ? $"SV{StructuralVariant.Id}" :
               throw new NullReferenceException("Specific variant is not set");
    }

    private string GetChromosome()
    {
        return Mutation != null ? Mutation.Chromosome :
               CopyNumberVariant != null ? CopyNumberVariant.Chromosome :
               StructuralVariant != null ? StructuralVariant.Chromosome :
               throw new NullReferenceException("Specific variant is not set");
    }

    private int GetStart()
    {
        return Mutation != null ? Mutation.Start :
               CopyNumberVariant != null ? CopyNumberVariant.Start :
               StructuralVariant != null ? StructuralVariant.Start :
               throw new NullReferenceException("Specific variant is not set");
    }

    private int GetEnd()
    {
        return Mutation != null ? Mutation.End :
               CopyNumberVariant != null ? CopyNumberVariant.End :
               StructuralVariant != null ? StructuralVariant.End :
               throw new NullReferenceException("Specific variant is not set");
    }
}
