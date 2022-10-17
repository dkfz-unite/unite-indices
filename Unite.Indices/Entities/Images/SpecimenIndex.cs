namespace Unite.Indices.Entities.Images;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public MutationIndex[] Mutations { get; set; }
    public CopyNumberVariantIndex[] CopyNumberVariants { get; set; }
    public StructuralVariantIndex[] StructuralVariants { get; set; }
}
