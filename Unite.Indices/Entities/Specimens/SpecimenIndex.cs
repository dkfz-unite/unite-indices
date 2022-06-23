namespace Unite.Indices.Entities.Specimens;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public DonorIndex Donor { get; set; }

    public SpecimenIndex Parent { get; set; }
    public SpecimenIndex[] Children { get; set; }
    public MutationIndex[] Mutations { get; set; }


    /// <summary>
    /// Number of donor mutations
    /// </summary>
    public int NumberOfMutations { get; set; }

    /// <summary>
    /// Number of expressed genes
    /// </summary>
    public int NumberOfGenes { get; set; }
}
