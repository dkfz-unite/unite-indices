namespace Unite.Indices.Entities.Basic.Omics;

public class ProteinNavIndex
{
    /// <summary>
    /// Protein id.
    /// </summary>
    [EntityKey]
    [Searchable]
    public int Id { get; set; }
}
