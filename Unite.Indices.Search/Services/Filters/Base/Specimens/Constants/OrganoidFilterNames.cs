namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class OrganoidFilterNames : SpecimenFilterNames
{
    protected override string Prefix => "Organoid";

    public string Medium => $"{Prefix}.Medium";
    public string Tumorigenicity => $"{Prefix}.Tumorigenicity";
}
