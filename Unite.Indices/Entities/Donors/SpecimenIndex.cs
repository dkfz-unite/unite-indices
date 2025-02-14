namespace Unite.Indices.Entities.Donors;

public class SpecimenIndex : Basic.Specimens.SpecimenNavIndex
{
    public SampleIndex[] Samples { get; set; }
}
