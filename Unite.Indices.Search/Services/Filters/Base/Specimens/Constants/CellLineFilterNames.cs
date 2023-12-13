namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class CellLineFilterNames : SpecimenFilterNames
{
    protected override string Prefix => "CellLine";

    public string Species => $"{Prefix}.Species";
    public string Type => $"{Prefix}.Type";
    public string CultureType => $"{Prefix}.CultureType";

    public string Name => $"{Prefix}.Name";
}
