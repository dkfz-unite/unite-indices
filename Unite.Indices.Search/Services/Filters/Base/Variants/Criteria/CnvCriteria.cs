namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record CnvCriteria : VariantCriteria
{
    public string[] Type { get; set; }
    public bool? Loh { get; set; }
    public bool? Del { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Type?.Length > 0
            || Loh != null
            || Del != null;
    }
}
