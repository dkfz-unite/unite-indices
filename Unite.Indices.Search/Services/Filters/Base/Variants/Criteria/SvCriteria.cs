using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record SvCriteria : VariantCriteria
{
    public ValuesCriteria<string> Type { get; set; }
    public BoolCriteria Inverted { get; set; }
}
