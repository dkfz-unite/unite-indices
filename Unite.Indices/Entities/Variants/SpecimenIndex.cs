namespace Unite.Indices.Entities.Variants;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public DonorIndex Donor { get; set; }

    public AnalysisIndex[] Analyses { get; set; }
    public ImageIndex[] Images { get; set; }
}
