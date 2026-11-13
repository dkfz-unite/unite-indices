namespace Unite.Indices.Entities.Basic.Specimens;

public class SpecimenNavIndex
{
    /// <summary>
    /// Specimen id.
    /// </summary>
    [Searchable]
    public int Id { get; set; }

    /// <summary>
    /// Specimen reference id. Depends on specimen type.
    /// </summary>
    [Searchable]
    public string ReferenceId { get; set; }

    /// <summary>
    /// Specimen type (<see cref="Constants.SpecimenType"/>).
    /// </summary>
    [Searchable]
    public string Type { get; set; }
}
