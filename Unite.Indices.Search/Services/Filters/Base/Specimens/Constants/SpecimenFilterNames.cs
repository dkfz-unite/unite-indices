namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class SpecimenFilterNames
{
    protected virtual string Prefix => "Specimen";

    public static string SpecimenId => "Specimen.Id";
    public static string SpecimenType => "Specimen.Type";

    public string Id => $"{Prefix}.Id";
    public string ReferenceId => $"{Prefix}.ReferenceId";

    public string MgmtStatus => $"{Prefix}.MgmtStatus";
    public string IdhStatus => $"{Prefix}.IhdStatus";
    public string IdhMutation => $"{Prefix}.IdhMutation";
    public string GeneExpressionSubtype => $"{Prefix}.GeneExpressionSubtype";
    public string MethylationSubtype => $"{Prefix}.MethylationSubtype";
    public string GcimpMethylation => $"{Prefix}.GcimpMethylation";

    public string  Intervention => $"{Prefix}.Intervention";

    public string Drug => $"{Prefix}.Drug";
    public string Dss => $"{Prefix}.Dss";
    public string DssS => $"{Prefix}.DssS";

    public string HasDrugs => $"{Prefix}.HasDrugs";
    public string HasSsms => $"{Prefix}.HasSsms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
    public string HasGeneExp => $"{Prefix}.HasGeneExp";
    public string HasGeneExpSc => $"{Prefix}.HasGeneExpSc";
    public string HasMeth => $"{Prefix}.HasMeth";
}
