namespace Unite.Indices.Search.Services.Filters.Base.Proteins.Constants;

public class ProteinFilterNames
{
    protected string Prefix => "Protein";

    public string Symbol => $"{Prefix}.Symbol";
    public string Accession => $"{Prefix}.Accession";
    public string Chromosome => $"{Prefix}.Chromosome";
    public string Position => $"{Prefix}.Position";
}
