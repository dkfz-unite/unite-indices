namespace Unite.Indices.Entities.Variants;

public class DonorIndex : Basic.Donors.DonorIndex
{
    public ImageIndex[] Images { get; set; }
    public SpecimenIndex[] Specimens { get; set; }
}
