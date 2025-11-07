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
    public ValuesCriteria<string> GeneKnockout { get; set; }

    public ValuesCriteria<string> Intervention { get; set; }

    public ValuesCriteria<string> Drug { get; set; }
    public RangeCriteria<double?> Dss { get; set; }
    public RangeCriteria<double?> DssS { get; set; }
}
