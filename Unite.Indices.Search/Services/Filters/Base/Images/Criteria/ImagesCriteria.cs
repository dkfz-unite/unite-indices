using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

public record ImagesCriteria : DataCriteria
{
    public ValuesCriteria<int> Id { get; set; }
    public ValuesCriteria<string> ReferenceId { get; set; }
    public ValuesCriteria<string> ImageType { get; set; }
}
