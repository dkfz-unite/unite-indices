namespace Unite.Indices.Entities.Basic.Images;

public class ImageNavIndex
{
    /// <summary>
    /// Image id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Image reference id. Depends on image type.
    /// </summary>
    /// <value></value>
    public string ReferenceId { get; set; }

    /// <summary>
    /// Image type (<see cref="Constants.ImageType"/>).
    /// </summary>
    public string Type { get; set; }
}
