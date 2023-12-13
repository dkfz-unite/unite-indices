namespace Unite.Indices.Search.Services.Filters.Base.Variants.Constants;

public class SsmFilterNames : VariantBaseFilterNames
{
    protected override string Prefix => "SSM";

    public string Type => $"{Prefix}.Type";
}
