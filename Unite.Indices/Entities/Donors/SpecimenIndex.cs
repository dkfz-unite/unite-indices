namespace Unite.Indices.Entities.Donors;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    public SampleIndex[] Samples { get; set; }
}
