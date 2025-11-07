namespace Unite.Indices.Entities.Basic.Specimens;

public class MolecularDataIndex
{
    public string MgmtStatus { get; set; }
    public string IdhStatus { get; set; }
    public string IdhMutation { get; set; }
    public string GeneExpressionSubtype { get; set; }
    public string MethylationSubtype { get; set; }
    public bool? GcimpMethylation { get; set; }
    public string[] GeneKnockouts { get; set; }
}
