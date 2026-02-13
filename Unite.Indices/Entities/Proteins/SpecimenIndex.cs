namespace Unite.Indices.Entities.Proteins;

public class SpecimenIndex : Basic.Specimens.SpecimenNavIndex
{
    /// <summary>
    /// Median centered Log2 intensity of the protein in this specimen.
    /// </summary>
    public double? Intensity { get; set; }
}
