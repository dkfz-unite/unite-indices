namespace Unite.Indices.Search.Services.Filters.Base.Images.Criteria;

public record ImageCriteria
{
    public int[] Id { get; set; }
    public string[] Type { get; set; }

    public bool? HasSsms { get; set; }
    public bool? HasCnvs { get; set; }
    public bool? HasSvs { get; set; }
    public bool? HasGeneExp { get; set; }
}
