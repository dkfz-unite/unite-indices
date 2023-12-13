namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record TissueCriteria : SpecimenBaseCriteria
{
    public string[] Type { get; set; }
    public string[] TumorType { get; set; }
    public string[] Source { get; set; }
}
