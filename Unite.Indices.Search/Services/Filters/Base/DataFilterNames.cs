namespace Unite.Indices.Search.Services.Filters.Base;

public class DataFilterNames
{
    protected virtual string Prefix => "Data";

    public string HasExp => $"{Prefix}.HasExp";
    public string HasExpSc => $"{Prefix}.HasExpSc";
    public string HasSms => $"{Prefix}.HasSms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
    public string HasMeth => $"{Prefix}.HasMeth";
    public string HasProt => $"{Prefix}.HasProt";
}
