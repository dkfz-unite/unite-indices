namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record OrganoidCriteria : SpecimenBaseCriteria
{
    public string[] Medium { get; set; }
    public bool? Tumorigenicity { get; set; }
}
