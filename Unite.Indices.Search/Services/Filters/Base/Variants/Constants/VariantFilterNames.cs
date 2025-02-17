namespace Unite.Indices.Search.Services.Filters.Base.Variants.Constants;

public class VariantFilterNames
{
    protected virtual string Prefix => "Variant";

    public string Chromosome => $"{Prefix}.Chromosome";
    public string Position => $"{Prefix}.Position";
    public string Length => $"{Prefix}.Length";
    public string Gene => $"{Prefix}.Gene";
    public string Impact => $"{Prefix}.Impact";
    public string Effect => $"{Prefix}.Effect";
}
