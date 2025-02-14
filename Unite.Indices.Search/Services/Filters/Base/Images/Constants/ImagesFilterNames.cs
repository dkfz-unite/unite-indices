namespace Unite.Indices.Search.Services.Filters.Base.Images.Constants;

public class ImagesFilterNames
{
    protected string Prefix => "Images";

    public string Id => $"{Prefix}.Id";
    public string ReferenceId => $"{Prefix}.ReferenceId";
    public string ImageType => $"{Prefix}.ImageType";

    public string HasExp => $"{Prefix}.HasExp";
    public string HasExpSc => $"{Prefix}.HasExpSc";
    public string HasSsms => $"{Prefix}.HasSsms";
    public string HasCnvs => $"{Prefix}.HasCnvs";
    public string HasSvs => $"{Prefix}.HasSvs";
    public string HasMeth => $"{Prefix}.HasMeth";
}
