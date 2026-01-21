using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record MaterialCriteria : SpecimenCriteria
{
    public ValuesCriteria<string> FixationType { get; set; }
    public ValuesCriteria<string> Source { get; set; }
}
