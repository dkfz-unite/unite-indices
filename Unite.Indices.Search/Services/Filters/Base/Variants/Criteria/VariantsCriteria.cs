using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record VariantsCriteria
{
    public ValuesCriteria<int> Id { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.IsNotEmpty() == true;
    }
}
