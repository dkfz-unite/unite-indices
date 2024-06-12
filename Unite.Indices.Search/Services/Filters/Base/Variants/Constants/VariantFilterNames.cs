namespace Unite.Indices.Search.Services.Filters.Base.Variants.Constants;

public class VariantFilterNames
{
    protected virtual string Prefix => "Variant";

    public static string VariantId => "Variant.VariantId";
    public static string VariantType => "Variant.VariantType";

    public string Id => $"{Prefix}.Id";
    public string Chromosome => $"{Prefix}.Chromosome";
    public string Position => $"{Prefix}.Position";
    public string Length => $"{Prefix}.Length";
    public string Impact => $"{Prefix}.Impact";
    public string Effect => $"{Prefix}.Effect";
}
