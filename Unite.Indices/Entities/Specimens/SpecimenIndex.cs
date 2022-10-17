namespace Unite.Indices.Entities.Specimens;

/// <summary>
/// Specimen centric index
/// </summary>
public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    /// <summary>
    /// Specimen donor
    /// </summary>
    public DonorIndex Donor { get; set; }


    /// <summary>
    /// Parent specimen
    /// </summary>
    public SpecimenIndex Parent { get; set; }

    /// <summary>
    /// Child specimens
    /// </summary>
    public SpecimenIndex[] Children { get; set; }


    /// <summary>
    /// Specimen images
    /// </summary>
    public ImageIndex[] Images { get; set; }


    /// <summary>
    /// Mutations in the specimen
    /// </summary>
    public MutationIndex[] Mutations { get; set; }

    /// <summary>
    /// Copy number variants in the specimen
    /// </summary>
    public CopyNumberVariantIndex[] CopyNumberVariants { get; set; }

    /// <summary>
    /// Structural variants in the specimen
    /// </summary>
    public StructuralVariantIndex[] StructuralVariants { get; set; }


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
