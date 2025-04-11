namespace Unite.Indices.Search.Services.Filters.Base.Projects.Constants;

public class ProjectsFilterNames
{
    protected string Prefix => "Projects";

    public string Id => $"{Prefix}.Id";
    public string Name => $"{Prefix}.Name";

    public string HasExp => $"{Prefix}.HasExp";
    public string HasExpSc => $"{Prefix}.HasExpSc";
    public string HasSms => $"{Prefix}.HasSms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
    public string HasMeth => $"{Prefix}.HasMeth";
}
