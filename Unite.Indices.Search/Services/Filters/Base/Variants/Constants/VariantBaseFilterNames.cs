namespace Unite.Indices.Search.Services.Filters.Base.Variants.Constants;

public abstract class VariantBaseFilterNames
{
    protected abstract string Prefix { get; }

    public static string VariantId => "Variant.Id";
    public static string VariantType => "Variant.VariantType";

    public string Id => $"{Prefix}.Id";
    public string Chromosome => $"{Prefix}.Chromosome";
    public string Position => $"{Prefix}.Position";
    public string Length => $"{Prefix}.Length";

    public string Impact => $"{Prefix}.Impact";
    public string Effect => $"{Prefix}.Effect";
}
