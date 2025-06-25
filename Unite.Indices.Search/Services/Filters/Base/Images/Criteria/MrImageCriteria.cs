using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

public record MrImageCriteria : ImageCriteria
{
    public RangeCriteria<double?> WholeTumor { get; set; }
    public RangeCriteria<double?> ContrastEnhancing { get; set; }
    public RangeCriteria<double?> NonContrastEnhancing { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || WholeTumor?.IsNotEmpty() == true
            || ContrastEnhancing?.IsNotEmpty() == true
            || NonContrastEnhancing?.IsNotEmpty() == true;
    }
}
