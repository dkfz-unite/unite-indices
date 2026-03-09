namespace Unite.Indices.Search.Services.Filters.Base.Images.Constants;

public class ImagesFilterNames : DataFilterNames
{
    protected override string Prefix => "Images";

    public string Id => $"{Prefix}.Id";
    public string ReferenceId => $"{Prefix}.ReferenceId";
    public string ImageType => $"{Prefix}.ImageType";
}
