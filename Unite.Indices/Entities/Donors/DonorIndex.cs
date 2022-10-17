namespace Unite.Indices.Entities.Donors;

public class DonorIndex : Basic.Donors.DonorIndex
{
    public ImageIndex[] Images { get; set; }
    public SpecimenIndex[] Specimens { get; set; }

    /// <summary>
    /// Number of donor images
    /// </summary>
    public int NumberOfImages { get; set; }

    /// <summary>
    /// Number of donor specimens
    /// </summary>
    public int NumberOfSpecimens { get; set; }

    /// <summary>
    /// Total number of genes affected by any variant across all specimens of the donor
    /// </summary>
    public int NumberOfGenes { get; set; }

    /// <summary>
    /// Total number of donor mutations across all specimens
    /// </summary>
    public int NumberOfMutations { get; set; }

    /// <summary>
    /// Total number of donor copy number variants across all specimens
    /// </summary>
    public int NumberOfCopyNumberVariants { get; set; }

    /// <summary>
    /// Total number of donor structural variants across all specimens
    /// </summary>
    public int NumberOfStructuralVariants { get; set; }
}
