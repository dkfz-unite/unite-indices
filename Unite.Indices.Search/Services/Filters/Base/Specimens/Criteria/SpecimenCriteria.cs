using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record SpecimenCriteria : SpecimensCriteria
{
    public string[] MgmtStatus { get; set; }
    public string[] IdhStatus { get; set; }
    public string[] IdhMutation { get; set; }
    public string[] GeneExpressionSubtype { get; set; }
    public string[] MethylationSubtype { get; set; }
    public bool? GcimpMethylation { get; set; }

    public string[] Intervention { get; set; }

    public string[] Drug { get; set; }
    public Range<double?> Dss { get; set; }
    public Range<double?> DssS { get; set; }

    
    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || ReferenceId?.Length > 0
            || MgmtStatus?.Length > 0
            || IdhStatus?.Length > 0
            || IdhMutation?.Length > 0
            || GeneExpressionSubtype?.Length > 0
            || MethylationSubtype?.Length > 0
            || GcimpMethylation != null
            || Intervention?.Length > 0
            || Drug?.Length > 0
            || Dss?.From != null
            || Dss?.To != null
            || DssS?.From != null
            || DssS?.To != null;
    }
}
