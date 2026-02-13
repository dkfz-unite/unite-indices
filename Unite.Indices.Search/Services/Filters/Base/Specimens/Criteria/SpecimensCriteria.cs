using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record SpecimensCriteria : DataCriteria
{
    public ValuesCriteria<int> Id { get; init; }
    public ValuesCriteria<string> ReferenceId { get; init; }
    public ValuesCriteria<string> SpecimenType { get; set; }
}
