namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record SvCriteria : VariantCriteria
{
    public string[] Type { get; set; }
    public bool? Inverted { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.Length > 0
            || Inverted != null;
    }
}
