namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record SvCriteria : VariantBaseCriteria
{
    public string[] Type { get; set; }
    public bool? Inverted { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.Any() == true
            || Inverted != null;
    }
}
