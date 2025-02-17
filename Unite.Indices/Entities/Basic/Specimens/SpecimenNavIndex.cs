namespace Unite.Indices.Entities.Basic.Specimens;

public class SpecimenNavIndex
{
    /// <summary>
    /// Specimen id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Specimen reference id. Depends on specimen type.
    /// </summary>
    public string ReferenceId { get; set; }

    /// <summary>
    /// Specimen type (<see cref="Constants.SpecimenType"/>).
    /// </summary>
    public string Type { get; set; }
}
