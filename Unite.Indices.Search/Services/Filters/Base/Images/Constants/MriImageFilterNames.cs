namespace Unite.Indices.Search.Services.Filters.Base.Images.Constants;

public class MriImageFilterNames : ImageFilterNames
{
    protected override string Prefix => "MRI";

    public string WholeTumor => $"{Prefix}.WholeTumor";
    public string ContrastEnhancing => $"{Prefix}.ContrastEnhancing";
    public string NonContrastEnhancing => $"{Prefix}.NonContrastEnhancing";
}
