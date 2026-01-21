namespace Unite.Indices.Entities.Basic.Specimens;

public class MolecularDataIndex
{
    public bool? MgmtStatus { get; set; }
    public bool? IdhStatus { get; set; }
    public string IdhMutation { get; set; }
    public bool? TertStatus { get; set; }
    public string TertMutation { get; set; }
    public string ExpressionSubtype { get; set; }
    public string MethylationSubtype { get; set; }
    public bool? GcimpMethylation { get; set; }
    public string[] GeneKnockouts { get; set; }
}
