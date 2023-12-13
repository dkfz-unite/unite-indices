namespace Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

public abstract record ImageBaseCriteria
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }
}
