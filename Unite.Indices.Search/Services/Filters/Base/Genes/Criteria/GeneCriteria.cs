namespace Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

public record GeneCriteria : GeneBaseCriteria
{
    public bool? HasSsms { get; set; }
    public bool? HasCnvs { get; set; }
    public bool? HasSvs { get; set; }
    public bool? HasGeneExp { get; set; }
}
