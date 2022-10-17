namespace Unite.Indices.Entities.Specimens;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public DonorIndex Donor { get; set; }

    public SpecimenIndex Parent { get; set; }
    public SpecimenIndex[] Children { get; set; }

    public ImageIndex[] Images { get; set; }
    public VariantIndex[] Variants { get; set; }


    /// <summary>
    /// Number of drugs tested
    /// </summary>
    public int NumberOfDrugs { get; set; }

    /// <summary>
    /// Number of genes affected by any variant in the specimen
    /// </summary>
    public int NumberOfGenes { get; set; }

    /// <summary>
    /// Number of mutations in the specimen
    /// </summary>
    public int NumberOfMutations { get; set; }

    /// <summary>
    /// Number of copy number variants in the specimen
    /// </summary>
    public int NumberOfCopyNumberVariants { get; set; }

    /// <summary>
    /// Number of structural variants in the specimen
    /// </summary>
    public int NumberStructuralVariants { get; set; }
}
