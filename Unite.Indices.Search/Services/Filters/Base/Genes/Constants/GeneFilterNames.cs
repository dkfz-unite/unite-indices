namespace Unite.Indices.Search.Services.Filters.Base.Genes.Constants;

public class GeneFilterNames
{
    protected string Prefix => "Gene";

    public string Symbol => $"{Prefix}.Symbol";
    public string Biotype => $"{Prefix}.Biotype";
    public string Chromosome => $"{Prefix}.Chromosome";
    public string Position => $"{Prefix}.Position";
}
