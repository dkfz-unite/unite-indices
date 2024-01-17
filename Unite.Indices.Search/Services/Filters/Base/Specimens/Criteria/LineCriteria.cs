namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record LineCriteria : SpecimenBaseCriteria
{
    public string[] CellsSpecies { get; set; }
    public string[] CellsType { get; set; }
    public string[] CultureType { get; set; }

    public string[] Name { get; set; }
}
