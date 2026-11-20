namespace Unite.Indices.Entities.Basic.Specimens;

public class LineIndex : SpecimenBaseIndex
{
    [Searchable]
    public string CellsSpecies { get; set; }
    [Searchable]
    public string CellsType { get; set; }
    [Searchable]
    public string CellsCultureType { get; set; }

    [Searchable]
    public string Name { get; set; }
    public string DepositorName { get; set; }
    public string DepositorEstablishment { get; set; }
    public DateOnly? EstablishmentDate { get; set; }

    public string PubmedLink { get; set; }
    public string AtccLink { get; set; }
    public string ExpasyLink { get; set; }
}
