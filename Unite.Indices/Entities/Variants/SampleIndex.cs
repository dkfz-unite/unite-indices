namespace Unite.Indices.Entities.Variants;

public class SampleIndex : Basic.Genome.Analysis.SampleIndex
{
    public DonorIndex Donor { get; set; }
    public SpecimenIndex Specimen { get; set; }

    public ImageIndex[] Images { get; set; }
}
