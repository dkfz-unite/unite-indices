namespace Unite.Indices.Entities.Basic.Specimens;

public class LineIndex : SpecimenBaseIndex
{
    public string CellsSpecies { get; set; }
    public string CellsType { get; set; }
    public string CellsCultureType { get; set; }

    public string Name { get; set; }
    public string DepositorName { get; set; }
    public string DepositorEstablishment { get; set; }
    public DateOnly? EstablishmentDate { get; set; }

    public string PubMedLink { get; set; }
    public string AtccLink { get; set; }
    public string ExPasyLink { get; set; }
}
