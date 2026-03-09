namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class SpecimensFilterNames : DataFilterNames
{
    protected override string Prefix => "Specimens";

    public string Id => $"{Prefix}.Id";
    public string ReferenceId => $"{Prefix}.ReferenceId";
    public string SpecimenType => $"{Prefix}.SpecimenType";
}
