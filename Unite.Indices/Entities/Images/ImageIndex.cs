namespace Unite.Indices.Entities.Images;

public class ImageIndex : Basic.Images.ImageIndex
{
    public int NumberOfSpecimens { get; set; }
    public int NumberOfGenes { get; set; }
    public int NumberOfMutations { get; set; }
    public int NumberOfCopyNumberVariants { get; set; }
    public int NumberOfStructuralVariants { get; set; }
    public bool HasGeneExpressions { get; set; }

    public DonorIndex Donor { get; set; }

    public SpecimenIndex[] Specimens { get; set; }
}
