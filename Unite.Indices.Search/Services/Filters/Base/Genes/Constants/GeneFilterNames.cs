namespace Unite.Indices.Search.Services.Filters.Base.Genes.Constants;

public class GeneFilterNames
{
    protected string Prefix => "Gene";

    public string Id => $"{Prefix}.Id";
    public string Symbol => $"{Prefix}.Symbol";
    public string Biotype => $"{Prefix}.Biotype";
    public string Chromosome => $"{Prefix}.Chromosome";
    public string Position => $"{Prefix}.Position";

    public string HasSsms => $"{Prefix}.HasSsms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
    public string HasGeneExp => $"{Prefix}.HasGeneExp";
}
