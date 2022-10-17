namespace Unite.Indices.Entities.Genes;

public class GeneIndex : Basic.Genome.GeneIndex
{
    public SpecimenIndex[] Specimens { get; set; }

    /// <summary>
    /// Total number of donors having this gene affected
    /// </summary>
    public int NumberOfDonors { get; set; }

    /// <summary>
    /// Total number of specimens having this gene affected
    /// </summary>
    public int NumberOfSpecimens { get; set; }

    /// <summary>
    /// Total number of mutations affecting this gene across all donors
    /// </summary>
    public int NumberOfMutations { get; set; }

    /// <summary>
    /// Total number of copy number variants affecting this gene across all donors
    /// </summary>
    public int NumberOfCopyNumberVariants { get; set; }

    /// <summary>
    /// Total number of structural variants affecting this gene across all donors
    /// </summary>
    public int NumberOfStructuralVariants { get; set; }
}
