namespace Unite.Indices.Entities.Basic.Specimens;

public class MaterialIndex : SpecimenBaseIndex
{
    public string Type { get; set; }
    public string FixationType { get; set; }
    public string TumorType { get; set; }
    public double? TumorGrade { get; set; }
    public string Source { get; set; }
}
