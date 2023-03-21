namespace Unite.Indices.Entities.Specimens;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public int NumberOfImages { get; set; }
    public int NumberOfGenes { get; set; }
    public int NumberOfMutations { get; set; }
    public int NumberOfCopyNumberVariants { get; set; }
    public int NumberOfStructuralVariants { get; set; }
    public bool HasDrugScreenings { get; set; }
    public bool HasGeneExpressions { get; set; }

    public DonorIndex Donor { get; set; }
    public SpecimenIndex Parent { get; set; }

    public ImageIndex[] Images { get; set; }
    public SampleIndex[] Samples { get; set; }
}
