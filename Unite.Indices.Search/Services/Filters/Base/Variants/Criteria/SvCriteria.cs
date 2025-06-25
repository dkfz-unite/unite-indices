using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record SvCriteria : VariantCriteria
{
    public ValuesCriteria<string> Type { get; set; }
    public BoolCriteria Inverted { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.IsNotEmpty() == true
            || Inverted?.IsNotEmpty() == true;
    }
}
