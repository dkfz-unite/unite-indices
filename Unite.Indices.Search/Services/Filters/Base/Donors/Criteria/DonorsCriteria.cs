using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

public record DonorsCriteria : DataCriteria
{
    public ValuesCriteria<int> Id { get; set; }
    public ValuesCriteria<string> ReferenceId { get; set; }
}
