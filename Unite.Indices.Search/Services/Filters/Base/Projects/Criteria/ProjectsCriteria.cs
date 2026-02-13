using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Projects.Criteria;

public record ProjectsCriteria : DataCriteria
{
    public ValuesCriteria<int> Id { get; set; }
    public ValuesCriteria<string> Name { get; set; }
}
