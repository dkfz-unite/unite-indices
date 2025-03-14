namespace Unite.Indices.Entities.Projects;

public class SpecimenIndex : Basic.Specimens.SpecimenNavIndex
{
    public SampleIndex[] Samples { get; set; }
}
