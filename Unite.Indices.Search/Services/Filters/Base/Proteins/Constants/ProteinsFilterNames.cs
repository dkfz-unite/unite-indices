namespace Unite.Indices.Search.Services.Filters.Base.Proteins.Constants;

public class ProteinsFilterNames
{
    protected string Prefix => "Proteins";

    public string Id => $"{Prefix}.Id";
    public string Expression => $"{Prefix}.Expression";

    public string HasSms => $"{Prefix}.HasSms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
}
