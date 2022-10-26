namespace Unite.Indices.Entities.Donors;

public class DonorIndex : Basic.Donors.DonorIndex
{
    public ImageIndex[] Images { get; set; }
    public SpecimenIndex[] Specimens { get; set; }

    /// <summary>
    /// Number of donor images
    /// </summary>
    public int NumberOfImages => GetNumberOfImages();

    /// <summary>
    /// Number of donor specimens
    /// </summary>
    public int NumberOfSpecimens => GetNumberOfSpecimens();

    /// <summary>
    /// Total number of genes affected by any variant across all specimens of the donor
    /// </summary>
    public int NumberOfGenes => GetNumberOfGenes();

    /// <summary>
    /// Total number of donor mutations across all specimens
    /// </summary>
    public int NumberOfMutations => GetNumberofMutations();

    /// <summary>
    /// Total number of donor copy number variants across all specimens
    /// </summary>
    public int NumberOfCopyNumberVariants => GetNumberOfCopyNumberVariants();

    /// <summary>
    /// Total number of donor structural variants across all specimens
    /// </summary>
    public int NumberOfStructuralVariants => GetNumberOfStructuralVariants();


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

    private int GetNumberOfCopyNumberVariants()
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
