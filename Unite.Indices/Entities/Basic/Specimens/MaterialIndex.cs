namespace Unite.Indices.Entities.Basic.Specimens;

public class MaterialIndex : SpecimenBaseIndex
{
    [Searchable]
    public string FixationType { get; set; }
    [Searchable]
    public string Source { get; set; }
}
