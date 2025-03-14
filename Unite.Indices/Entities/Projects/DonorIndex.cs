namespace Unite.Indices.Entities.Projects;

public class DonorIndex : Basic.Donors.DonorNavIndex
{
    public ImageIndex[] Images { get; set; }
    public SpecimenIndex[] Specimens { get; set; }
}
