namespace Unite.Indices.Search.Services.Filters.Base.Variants.Constants;

public class SvFilterNames : VariantFilterNames
{
    protected override string Prefix => "SV";

    public string Type => $"{Prefix}.Type";
    public string Inverted => $"{Prefix}.Inverted";
}
