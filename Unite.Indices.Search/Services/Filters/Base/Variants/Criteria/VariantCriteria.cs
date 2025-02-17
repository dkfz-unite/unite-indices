using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record VariantCriteria : VariantsCriteria
{
    public string[] Chromosome { get; set; }
    public Range<double?> Position { get; set; }
    public Range<int?> Length { get; set; }

    public string[] Gene { get; set; }
    public string[] Impact { get; set; }
    public string[] Effect { get; set; }


    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Chromosome?.Length > 0
            || Position?.From != null
            || Position?.To != null
            || Length?.From != null
            || Length?.To != null
            || Gene?.Length > 0
            || Impact?.Length > 0
            || Effect?.Length > 0;
    }
}
