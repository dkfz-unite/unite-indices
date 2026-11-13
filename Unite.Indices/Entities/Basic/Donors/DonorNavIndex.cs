namespace Unite.Indices.Entities.Basic.Donors;

public class DonorNavIndex
{
    /// <summary>
    /// Donor id.
    /// </summary>
    [EntityKey]
    [Searchable]
    public int Id { get; set; }

    /// <summary>
    /// Reference id.
    /// </summary>
    [Searchable]
    public string ReferenceId { get; set; }
}
