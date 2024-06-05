using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public abstract record SpecimenBaseCriteria
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }

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
}
