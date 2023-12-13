namespace Unite.Indices.Entities.Basic.Images;

public class ImageIndex
{
    /// <summary>
    /// Image id.
    /// </summary> 
    public int Id { get; set; }

    /// <summary>
    /// Specific image reference Id. Depends on image type. Should be set during indexing.
    /// </summary>
    /// <value></value>
    public string ReferenceId { get; set; }

    /// <summary>
    /// Type of the image. Should be set during indexing (<see cref="Constants.ImageType"/>).
    /// </summary>
    public string Type { get; set; }


    public MriImageIndex Mri { get; set; }
}
