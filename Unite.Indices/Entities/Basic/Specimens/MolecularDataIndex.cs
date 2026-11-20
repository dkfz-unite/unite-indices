namespace Unite.Indices.Entities.Basic.Specimens;

public class MolecularDataIndex
{
    [Searchable]
    public bool? MgmtStatus { get; set; }
    [Searchable]
    public bool? IdhStatus { get; set; }
    [Searchable]
    public string IdhMutation { get; set; }
    [Searchable]
    public bool? TertStatus { get; set; }
    [Searchable]
    public string TertMutation { get; set; }
    [Searchable]
    public string ExpressionSubtype { get; set; }
    [Searchable]
    public string MethylationSubtype { get; set; }
    [Searchable]
    public bool? GcimpMethylation { get; set; }
    [Searchable]
    public string[] GeneKnockouts { get; set; }
}
