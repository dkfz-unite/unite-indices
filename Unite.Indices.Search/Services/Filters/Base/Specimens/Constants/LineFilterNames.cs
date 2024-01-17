namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class LineFilterNames : SpecimenFilterNames
{
    protected override string Prefix => "Line";

    public string CellsSpecies => $"{Prefix}.CellsSpecies";
    public string CellsType => $"{Prefix}.CellsType";
    public string CellsCultureType => $"{Prefix}.CellsCultureType";

    public string Name => $"{Prefix}.Name";
}
