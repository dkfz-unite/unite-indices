namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class SpecimenFilterNames
{
    protected virtual string Prefix => "Specimen";

    public string Id => $"{Prefix}.Id";
    public string ReferenceId => $"{Prefix}.ReferenceId";

    public string Condition => $"{Prefix}.Condition";
    public string TumorType => $"{Prefix}.TumorType";
    public string TumorGrade => $"{Prefix}.TumorGrade";
    public string TumorSuperfamily => $"{Prefix}.TumorSuperfamily";
    public string TumorFamily => $"{Prefix}.TumorFamily";
    public string TumorClass => $"{Prefix}.TumorClass";
    public string TumorSubclass => $"{Prefix}.TumorSubclass";

    public string MgmtStatus => $"{Prefix}.MgmtStatus";
    public string IdhStatus => $"{Prefix}.IhdStatus";
    public string IdhMutation => $"{Prefix}.IdhMutation";
    public string TertStatus => $"{Prefix}.TertStatus";
    public string TertMutation => $"{Prefix}.TertMutation";
    public string ExpressionSubtype => $"{Prefix}.ExpressionSubtype";
    public string MethylationSubtype => $"{Prefix}.MethylationSubtype";
    public string GcimpMethylation => $"{Prefix}.GcimpMethylation";
    public string GeneKnockout => $"{Prefix}.GeneKnockout";

    public string  Intervention => $"{Prefix}.Intervention";

    public string Drug => $"{Prefix}.Drug";
    public string Dss => $"{Prefix}.Dss";
    public string DssS => $"{Prefix}.DssS";
}
