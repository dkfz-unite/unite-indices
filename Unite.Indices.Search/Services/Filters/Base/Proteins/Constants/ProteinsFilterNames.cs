namespace Unite.Indices.Search.Services.Filters.Base.Proteins.Constants;

public class ProteinsFilterNames
{
    protected string Prefix => "Proteins";

    public string Id => $"{Prefix}.Id";
    public string Intensity => $"{Prefix}.Intensity";

    public string HasSms => $"{Prefix}.HasSms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
}
