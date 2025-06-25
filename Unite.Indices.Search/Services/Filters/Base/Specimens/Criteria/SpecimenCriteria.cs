using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record SpecimenCriteria : SpecimensCriteria
{
    public ValuesCriteria<string> MgmtStatus { get; set; }
    public ValuesCriteria<string> IdhStatus { get; set; }
    public ValuesCriteria<string> IdhMutation { get; set; }
    public ValuesCriteria<string> GeneExpressionSubtype { get; set; }
    public ValuesCriteria<string> MethylationSubtype { get; set; }
    public BoolCriteria GcimpMethylation { get; set; }

    public ValuesCriteria<string> Intervention { get; set; }

    public ValuesCriteria<string> Drug { get; set; }
    public RangeCriteria<double?> Dss { get; set; }
    public RangeCriteria<double?> DssS { get; set; }

    
    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || MgmtStatus?.IsNotEmpty() == true
            || IdhStatus?.IsNotEmpty() == true
            || IdhMutation?.IsNotEmpty() == true
            || GeneExpressionSubtype?.IsNotEmpty() == true
            || MethylationSubtype?.IsNotEmpty() == true
            || GcimpMethylation?.IsNotEmpty() == true
            || Intervention?.IsNotEmpty() == true
            || Drug?.IsNotEmpty() == true
            || Dss?.IsNotEmpty() == true
            || DssS?.IsNotEmpty() == true;
    }
}
