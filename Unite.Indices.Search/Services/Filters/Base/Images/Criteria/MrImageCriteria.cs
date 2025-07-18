using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

public record MrImageCriteria : ImageCriteria
{
    public RangeCriteria<double?> WholeTumor { get; set; }
    public RangeCriteria<double?> ContrastEnhancing { get; set; }
    public RangeCriteria<double?> NonContrastEnhancing { get; set; }
}
