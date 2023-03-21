namespace Unite.Indices.Entities.Donors;

public class DonorIndex : Basic.Donors.DonorIndex
{
    public int NumberOfImages { get; set; }
    public int NumberOfSpecimens { get; set; }
    public int NumberOfGenes { get; set; }
    public int NumberOfMutations { get; set; }
    public int NumberOfCopyNumberVariants { get; set; }
    public int NumberOfStructuralVariants { get; set; }
    public bool HasGeneExpressions { get; set; }

    public ImageIndex[] Images { get; set; }
    public SpecimenIndex[] Specimens { get; set; }
}
