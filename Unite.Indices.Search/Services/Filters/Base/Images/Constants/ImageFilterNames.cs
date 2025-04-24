namespace Unite.Indices.Search.Services.Filters.Base.Images.Constants;

public class ImageFilterNames
{
    protected virtual string Prefix => "Image";

    public string Id => $"{Prefix}.Id";
    public string ReferenceId => $"{Prefix}.ReferenceId";
}
