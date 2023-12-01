
using Unite.Indices.Search.Engine.Enums;

namespace Unite.Indices.Search.Services.Context;

public class ImageSearchContext
{
    public ImageType? ImageType { get; set; }


    public ImageSearchContext()
    {
    }

    public ImageSearchContext(ImageType imageType) : base()
    {
        ImageType = imageType;
    }
}
