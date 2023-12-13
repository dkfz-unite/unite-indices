namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class TissueFilterNames : SpecimenFilterNames
{
    protected override string Prefix => "Tissue";

    public string Type => $"{Prefix}.Type";
    public string TumorType => $"{Prefix}.TumorType";
    public string Source => $"{Prefix}.Source";
}
