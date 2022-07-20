namespace Unite.Indices.Entities.Basic.Specimens;

public class CellLineIndex
{
    public string ReferenceId { get; set; }

    public string Species { get; set; }
    public string Type { get; set; }
    public string CultureType { get; set; }

    public string Name { get; set; }
    public string DepositorName { get; set; }
    public string DepositorEstablishment { get; set; }
    public DateTime? EstablishmentDate { get; set; }

    public string PubMedLink { get; set; }
    public string AtccLink { get; set; }
    public string ExPasyLink { get; set; }

    public MolecularDataIndex MolecularData { get; set; }

    public DrugScreeningIndex[] DrugScreenings { get; set; }
}
