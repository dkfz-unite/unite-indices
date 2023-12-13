namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record CellLineCriteria : SpecimenBaseCriteria
{
    public string[] Species { get; set; }
    public string[] Type { get; set; }
    public string[] CultureType { get; set; }

    public string[] Name { get; set; }
}
