namespace Unite.Indices.Entities.Variants.SSM;

public class VariantIndex : Basic.Genome.Variants.SSM.VariantIndex
{
    public DonorIndex[] Donors { get; set; }

    /// <summary>
    /// Total number of donors affected by the mutation
    /// </summary>
    public int NumberOfDonors { get; set; }

    /// <summary>
    /// Total number of specimens affected by the mutation across all donors
    /// </summary>
    public int NumberOfSpecimens { get; set; }
}
