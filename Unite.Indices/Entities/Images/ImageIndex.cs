namespace Unite.Indices.Entities.Images;

public class ImageIndex : Basic.Images.ImageIndex
{
    private int? _numberOfGenes;
    private int? _numberOfMutations;
    private int? _numberOfCopyNumberVariants;
    private int? _numberOfStructuralVariants;


    public DonorIndex Donor { get; set; }

    public SpecimenIndex[] Specimens { get; set; }


    /// <summary>
    /// Total number of genes affected by any variant across all tissues of the image donor
    /// </summary>
    public int NumberOfGenes
    {
        get => _numberOfGenes ?? GetnumberOfGenes();
        set => _numberOfGenes = value;
    }

    /// <summary>
    /// Total number of mutations across all tissues of the image donor
    /// </summary>
    public int NumberOfMutations
    {
        get => _numberOfMutations ?? GetNumberofMutations();
        set => _numberOfMutations = value;
    }

    /// <summary>
    /// Total number of copy number variants across all tissues of the image donor
    /// </summary>
    public int NumberOfCopyNumberVariants
    {
        get => _numberOfCopyNumberVariants ?? GetNumberofCopyNumberVariants();
        set => _numberOfCopyNumberVariants = value;
    }

    /// <summary>
    /// Total number of structural variants across all tissues of the image donor
    /// </summary>
    public int NumberOfStructuralVariants
    {
        get => _numberOfStructuralVariants ?? GetNumberOfStructuralVariants();
        set => _numberOfStructuralVariants = value;
    }


    private int GetnumberOfGenes()
    {
        return Specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.AffectedFeatures != null)
            .SelectMany(variant => variant.AffectedFeatures)
            .Where(feature => feature.Gene != null)
            .Select(feature => feature.Gene)
            .DistinctBy(gene => gene.Id)
            .Count() ?? 0;
    }

    private int GetNumberofMutations()
    {
        return Specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.Mutation != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberofCopyNumberVariants()
    {
        return Specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.CopyNumberVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfStructuralVariants()
    {
        return Specimens?
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.StructuralVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }
}
