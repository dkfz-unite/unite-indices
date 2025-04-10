namespace Unite.Indices.Search.Services.Filters.Base.Images.Constants;

public class MrImageFilterNames : ImageFilterNames
{
    protected override string Prefix => "MR";

    public string WholeTumor => $"{Prefix}.WholeTumor";
    public string ContrastEnhancing => $"{Prefix}.ContrastEnhancing";
    public string NonContrastEnhancing => $"{Prefix}.NonContrastEnhancing";
}
