namespace Unite.Indices.Entities.Basic.Images;

public class ImageIndex
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public string Type { get; set; }
    public int? ScanningDay { get; set; }

    public MriImageIndex Mri { get; set; }
}
