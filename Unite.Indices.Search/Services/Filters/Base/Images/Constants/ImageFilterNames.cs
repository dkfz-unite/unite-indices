namespace Unite.Indices.Search.Services.Filters.Base.Images.Constants;

public class ImageFilterNames
{
    protected virtual string Prefix => "Image";

    public static string ImageId => "Image.ImageId";
    public static string ImageType => "Image.ImageType";

    public string Id => $"{Prefix}.Id";
    public string ReferenceId => $"{Prefix}.ReferenceId";

    public string HasSsms => $"{Prefix}.HasSsms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
    public string HasGeneExp => $"{Prefix}.HasGeneExp";
}
