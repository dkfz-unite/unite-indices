using Unite.Indices.Search.Services.Criteria.Models;

namespace Unite.Indices.Search.Services.Criteria;

public record MriImageCriteria : ImageCriteriaBase
{
    public Range<double?> WholeTumor { get; set; }
    public Range<double?> ContrastEnhancing { get; set; }
    public Range<double?> NonContrastEnhancing { get; set; }
}
