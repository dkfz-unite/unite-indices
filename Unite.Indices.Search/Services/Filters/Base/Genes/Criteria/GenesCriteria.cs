namespace Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

public record GenesCriteria
{
    public int[] Id { get; set; }

    public bool? HasSsms { get; set; }
    public bool? HasCnvs { get; set; }
    public bool? HasSvs { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.Length > 0
            || HasSsms != null
            || HasCnvs != null
            || HasSvs != null;
    }
}
