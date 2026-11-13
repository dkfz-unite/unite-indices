namespace Unite.Indices.Entities.Basic.Images;

public class ImageNavIndex
{
    /// <summary>
    /// Image id.
    /// </summary>
    [Searchable]
    public int Id { get; set; }

    /// <summary>
    /// Image reference id. Depends on image type.
    /// </summary>
    /// <value></value>
    [Searchable]
    public string ReferenceId { get; set; }

    /// <summary>
    /// Image type (<see cref="Constants.ImageType"/>).
    /// </summary>
    [Searchable]
    public string Type { get; set; }
}
