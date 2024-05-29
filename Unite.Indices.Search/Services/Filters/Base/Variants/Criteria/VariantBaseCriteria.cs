using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public abstract record VariantBaseCriteria
{
    public string[] Id { get; set; }

    public string[] Chromosome { get; set; }
    public Range<double?> Position { get; set; }
    public Range<int?> Length { get; set; }

    public string[] Impact { get; set; }
    public string[] Effect { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.Any() == true
            || Chromosome?.Any() == true
            || Position?.From != null
            || Position?.To != null
            || Length?.From != null
            || Length?.To != null
            || Impact?.Any() == true
            || Effect?.Any() == true;
    }
}
