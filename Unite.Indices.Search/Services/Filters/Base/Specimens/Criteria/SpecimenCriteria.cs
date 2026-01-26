using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record SpecimenCriteria : SpecimensCriteria
{
    public ValuesCriteria<string> Category { get; set; }
    public ValuesCriteria<string> TumorType { get; set; }
    public RangeCriteria<double?> TumorGrade { get; set; }
    public ValuesCriteria<string> TumorSuperfamily { get; set; }
    public ValuesCriteria<string> TumorFamily { get; set; }
    public ValuesCriteria<string> TumorClass { get; set; }
    public ValuesCriteria<string> TumorSubClass { get; set; }
    public BoolCriteria MgmtStatus { get; set; }
    public BoolCriteria IdhStatus { get; set; }
    public ValuesCriteria<string> IdhMutation { get; set; }
    public BoolCriteria TertStatus { get; set; }
    public ValuesCriteria<string> TertMutation { get; set; }
    public ValuesCriteria<string> ExpressionSubtype { get; set; }
    public ValuesCriteria<string> MethylationSubtype { get; set; }
    public BoolCriteria GcimpMethylation { get; set; }
    public ValuesCriteria<string> GeneKnockout { get; set; }

    public ValuesCriteria<string> Intervention { get; set; }

    public ValuesCriteria<string> Drug { get; set; }
    public RangeCriteria<double?> Dss { get; set; }
    public RangeCriteria<double?> DssS { get; set; }
}
