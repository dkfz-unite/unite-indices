namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class MaterialFilterNames : SpecimenFilterNames
{
    protected override string Prefix => "Material";

    public string Type => $"{Prefix}.Type";
    public string TumorType => $"{Prefix}.TumorType";
    public string Source => $"{Prefix}.Source";
}
