namespace Unite.Indices.Entities.Donors;

public class DonorIndex : Basic.Donors.DonorIndex
{
    private int? _numberOfImages;
    private int? _numberOfSpecimens;
    private int? _numberOfGenes;
    private int? _numberOfMutations;
    private int? _numberOfCopyNumberVariants;
    private int? _numberOfStructuralVariants;


    public ImageIndex[] Images { get; set; }
    public SpecimenIndex[] Specimens { get; set; }


    /// <summary>
    /// Number of donor images
    /// </summary>
    public int NumberOfImages
    {
        get => _numberOfImages ?? GetNumberOfImages();
        set => _numberOfImages = value;
    }

    /// <summary>
    /// Number of donor specimens
    /// </summary>
    public int NumberOfSpecimens
    {
        get => _numberOfSpecimens ?? GetNumberOfSpecimens();
        set => _numberOfSpecimens = value;
    }

    /// <summary>
    /// Total number of genes affected by any variant across all specimens of the donor
    /// </summary>
    public int NumberOfGenes
    {
        get => _numberOfGenes ?? GetNumberOfGenes();
        set => _numberOfGenes = value;
    }

    /// <summary>
    /// Total number of donor mutations across all specimens
    /// </summary>
    public int NumberOfMutations
    {
        get => _numberOfMutations ?? GetNumberofMutations();
        set => _numberOfMutations = value;
    }

    /// <summary>
    /// Total number of donor copy number variants across all specimens
    /// </summary>
    public int NumberOfCopyNumberVariants
    {
        get => _numberOfCopyNumberVariants ?? GetNumberOfCopyNumberVariants();
        set => _numberOfCopyNumberVariants = value;
    }

    /// <summary>
    /// Total number of donor structural variants across all specimens
    /// </summary>
    public int NumberOfStructuralVariants
    {
        get => _numberOfStructuralVariants ?? GetNumberOfStructuralVariants();
        set => _numberOfStructuralVariants = value;
    }


    private int GetNumberOfImages()
    {
        return Images?.Length ?? 0;
    }

    private int GetNumberOfSpecimens()
    {
        return Specimens?.Length ?? 0;
    }

    private int GetNumberOfGenes()
    {
        return Specimens?
            .Where(specimen => specimen.Variants != null)
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
            .Where(specimen => specimen.Variants != null)
            .SelectMany(specimen => specimen.Variants)?
            .Where(variant => variant.Mutation != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfCopyNumberVariants()
    {
        return Specimens?
            .Where(specimen => specimen.Variants != null)
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.CopyNumberVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }

    private int GetNumberOfStructuralVariants()
    {
        return Specimens?
            .Where(specimen => specimen.Variants != null)
            .SelectMany(specimen => specimen.Variants)
            .Where(variant => variant.StructuralVariant != null)
            .DistinctBy(variant => variant.Id)
            .Count() ?? 0;
    }
}
