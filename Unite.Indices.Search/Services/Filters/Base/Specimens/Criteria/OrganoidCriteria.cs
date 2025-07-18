using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record OrganoidCriteria : SpecimenCriteria
{
    public ValuesCriteria<string> Medium { get; set; }
    public BoolCriteria Tumorigenicity { get; set; }
}
