namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class SpecimensFilterNames
{
    protected virtual string Prefix => "Specimens";

    public string Id => $"{Prefix}.Id";
    public string ReferenceId => $"{Prefix}.ReferenceId";
    public string SpecimenType => $"{Prefix}.SpecimenType";

    public string HasExp => $"{Prefix}.HasExp";
    public string HasExpSc => $"{Prefix}.HasExpSc";
    public string HasSsms => $"{Prefix}.HasSsms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
    public string HasMeth => $"{Prefix}.HasMeth";
}
