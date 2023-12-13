namespace Unite.Indices.Search.Services.Filters.Base.Variants.Constants;

public class CnvFilterNames : VariantBaseFilterNames
{
    protected override string Prefix => "CNV";

    public string Type => $"{Prefix}.Type";
    public string Loh => $"{Prefix}.Loh";
    public string HomoDel => $"{Prefix}.HomoDel";
}
