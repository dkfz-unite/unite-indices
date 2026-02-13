namespace Unite.Indices.Search.Services.Filters.Base.Genes.Constants;

public class GenesFilterNames
{
    protected string Prefix => "Genes";

    public string Id => $"{Prefix}.Id";
    public string TPM => $"{Prefix}.TPM";
    public string FPKM => $"{Prefix}.FPKM";

    public string HasSms => $"{Prefix}.HasSms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
}
