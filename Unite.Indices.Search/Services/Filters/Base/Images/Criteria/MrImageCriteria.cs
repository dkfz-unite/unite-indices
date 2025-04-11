using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

public record MrImageCriteria : ImageCriteria
{
    public Range<double?> WholeTumor { get; set; }
    public Range<double?> ContrastEnhancing { get; set; }
    public Range<double?> NonContrastEnhancing { get; set; }

    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || WholeTumor?.From != null
            || WholeTumor?.To != null
            || ContrastEnhancing?.From != null
            || ContrastEnhancing?.To != null
            || NonContrastEnhancing?.From != null
            || NonContrastEnhancing?.To != null;
    }
}
