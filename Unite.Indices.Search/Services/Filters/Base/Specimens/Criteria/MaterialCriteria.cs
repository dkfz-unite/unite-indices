namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record MaterialCriteria : SpecimenBaseCriteria
{
    public string[] Type { get; set; }
    public string[] TumorType { get; set; }
    public string[] Source { get; set; }
}
