using Unite.Indices.Search.Services.Filters.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record VariantCriteria : VariantsCriteria
{
    public ValuesCriteria<string> Chromosome { get; set; }
    public RangeCriteria<double?> Position { get; set; }
    public RangeCriteria<int?> Length { get; set; }

    public ValuesCriteria<string> Gene { get; set; }
    public ValuesCriteria<string> Impact { get; set; }
    public ValuesCriteria<string> Effect { get; set; }


    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Chromosome?.IsNotEmpty() == true
            || Position?.IsNotEmpty() == true
            || Length?.IsNotEmpty() == true
            || Gene?.IsNotEmpty() == true
            || Impact?.IsNotEmpty() == true
            || Effect?.IsNotEmpty() == true;
    }
}
