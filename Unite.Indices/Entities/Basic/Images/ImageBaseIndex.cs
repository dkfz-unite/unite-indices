namespace Unite.Indices.Entities.Basic.Images;

public abstract class ImageBaseIndex
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public int? CreationDay { get; set; }
}
