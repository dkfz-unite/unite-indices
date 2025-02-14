namespace Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

public record ImageCriteria : ImagesCriteria
{
    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty();
    }
}
