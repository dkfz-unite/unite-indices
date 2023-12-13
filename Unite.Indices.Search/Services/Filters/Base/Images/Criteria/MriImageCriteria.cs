using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

public record MriImageCriteria : ImageBaseCriteria
{
    public Range<double?> WholeTumor { get; set; }
    public Range<double?> ContrastEnhancing { get; set; }
    public Range<double?> NonContrastEnhancing { get; set; }
}
