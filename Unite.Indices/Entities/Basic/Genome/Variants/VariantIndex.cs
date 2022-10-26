namespace Unite.Indices.Entities.Basic.Genome.Variants;

public class VariantIndex
{
    private string _id;
    private string _chromosome;
    private int? _start;
    private int? _end;


    public string Id
    {
        get => _id ?? GetId();
        set => _id = value;
    }

    public string Chromosome
    {
        get => _chromosome ?? GetChromosome();
        set => _chromosome = value;
    }

    public int Start
    {
        get => _start ?? GetStart();
        set => _start = value;
    }

    public int End
    {
        get => _end ?? GetEnd();
        set => _end = value;
    }


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
