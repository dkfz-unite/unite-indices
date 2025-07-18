using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record CnvCriteria : VariantCriteria
{
    public ValuesCriteria<string> Type { get; set; }
    public BoolCriteria Loh { get; set; }
    public BoolCriteria Del { get; set; }
}
