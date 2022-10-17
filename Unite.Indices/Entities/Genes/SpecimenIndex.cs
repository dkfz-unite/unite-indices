namespace Unite.Indices.Entities.Genes;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public DonorIndex Donor { get; set; }

    public ImageIndex[] Images { get; set; }

    public MutationIndex[] Mutations { get; set; }
    public CopyNumberVariantIndex[] CopyNumberVariants { get; set; }
    public StructuralVariantIndex[] StructuralVariants { get; set; }
}
