namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Constants;

public class MaterialFilterNames : SpecimenFilterNames
{
    protected override string Prefix => "Material";

    public string FixationType => $"{Prefix}.FixationType";
    
    public string Source => $"{Prefix}.Source";
}
