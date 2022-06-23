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
    /// Number of donor mutations
    /// </summary>
    public int NumberOfMutations { get; set; }

    /// <summary>
    /// Number of mutated genes
    /// </summary>
    public int NumberOfGenes { get; set; }
}
