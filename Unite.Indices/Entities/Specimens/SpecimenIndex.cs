namespace Unite.Indices.Entities.Specimens;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    private int? _numberOfDrugs;
    private int? _numberOfGenes;
    private int? _numberOfMutations;
    private int? _numberOfCopyNumberVariants;
    private int? _numberOfStructuralVariants;


    public DonorIndex Donor { get; set; }

    public SpecimenIndex Parent { get; set; }

    public ImageIndex[] Images { get; set; }
    public VariantIndex[] Variants { get; set; }


    /// <summary>
    /// Number of drugs tested
    /// </summary>
    public int NumberOfDrugs
    {
        get => _numberOfDrugs ?? GetNumberOfDrugs();
        set => _numberOfDrugs = value;
    }

    /// <summary>
    /// Number of genes affected by any variant in the specimen
    /// </summary>
    public int NumberOfGenes
    {
        get => _numberOfGenes ?? GetNumberOfGenes();
        set => _numberOfGenes = value;
    }

    /// <summary>
    /// Number of mutations in the specimen
    /// </summary>
    public int NumberOfMutations
    {
        get => _numberOfMutations ?? GetNumberOfMutations();
        set => _numberOfMutations = value;
    }

    /// <summary>
    /// Number of copy number variants in the specimen
    /// </summary>
    public int NumberOfCopyNumberVariants
    {
        get => _numberOfCopyNumberVariants ?? GetNumberOfCopyNumberVariants();
        set => _numberOfCopyNumberVariants = value;
    }

    /// <summary>
    /// Number of structural variants in the specimen
    /// </summary>
    public int NumberStructuralVariants
    {
        get => _numberOfStructuralVariants ?? GetNumberOfStructuralVariants();
        set => _numberOfStructuralVariants = value;
    }


    private int GetNumberOfDrugs()
    {
        return DrugScreenings?
            .DistinctBy(screening => screening.Drug)
            .Count() ?? 0;
    }

    private int GetNumberOfGenes()
    {
        return Variants?
            .Where(variant => variant.AffectedFeatures != null)
            .SelectMany(variant => variant.AffectedFeatures)
            .Where(feature => feature.Gene != null)
            .Select(feature => feature.Gene)
            .DistinctBy(gene => gene.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfMutations()
    {
        return Variants?
            .Where(variant => variant.Mutation != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCopyNumberVariants()
    {
        return Variants?
            .Where(variant => variant.CopyNumberVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfStructuralVariants()
    {
        return Variants?
            .Where(variant => variant.StructuralVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }
}
