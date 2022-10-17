namespace Unite.Indices.Entities.Variants;

public class VariantIndex : Basic.Genome.Variants.VariantIndex
{
    public DonorIndex[] Donors { get; set; }

    /// <summary>
    /// Total number of donors affected by the variant
    /// </summary>
    public int NumberOfDonors { get; set; }

    /// <summary>
    /// Total number of specimens affected by the variant across all donors
    /// </summary>
    public int NumberOfSpecimens { get; set; }

    /// <summary>
    /// Total number of genes affected by the variant across all donors
    /// </summary>
    public int NumberOfGenes { get; set; }
}
