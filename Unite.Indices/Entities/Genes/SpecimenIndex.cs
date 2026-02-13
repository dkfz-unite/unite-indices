namespace Unite.Indices.Entities.Genes;

public class SpecimenIndex : Basic.Specimens.SpecimenNavIndex
{
    // public SampleIndex[] Samples { get; set; }
    public double? TPM { get; set; }
    public double? FPKM { get; set; }
}
