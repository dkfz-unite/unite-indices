using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record VariantsCriteria : CriteriaCollection
{
    public ValuesCriteria<int> Id { get; set; }
}
