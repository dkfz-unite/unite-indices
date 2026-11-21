namespace Unite.Indices.Entities.Basic.Specimens;

public class OrganoidIndex : SpecimenBaseIndex
{
    public int? ImplantedCellsNumber { get; set; }
    [Searchable]
    public bool? Tumorigenicity { get; set; }
    [Searchable]
    public string Medium { get; set; }
}
