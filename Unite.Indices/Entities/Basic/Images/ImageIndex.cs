using Unite.Indices.Entities.Basic.Images.Constants;

namespace Unite.Indices.Entities.Basic.Images;

public class ImageIndex
{
    private string _referenceId;
    private string _type;

    public int Id { get; set; }
    public string ReferenceId { get => _referenceId ?? GetImageReferenceId(); set => _referenceId = value; }
    public string Type { get => _type ?? GetImageType(); set => _type = value; }
    public int? ScanningDay { get; set; }

    public MriImageIndex Mri { get; set; }


    private string GetImageReferenceId()
    {
        return Mri != null ? $"{Mri.ReferenceId}" :
               throw new NullReferenceException("Specific image is not set.");
    }

    private string GetImageType()
    {
        return Mri != null ? ImageTypes.MRI :
               throw new NullReferenceException("Specific image is not set.");
    }
}
