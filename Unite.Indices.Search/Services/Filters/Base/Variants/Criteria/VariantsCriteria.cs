namespace Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;

public record VariantsCriteria
{
    public int[] Id { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.Length > 0;
    }
}
