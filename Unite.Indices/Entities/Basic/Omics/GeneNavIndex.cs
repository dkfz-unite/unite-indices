namespace Unite.Indices.Entities.Basic.Omics;

public class GeneNavIndex
{
    /// <summary>
    /// Gene id.
    /// </summary>
    [EntityKey]
    [Searchable]
    public int Id { get; set; }
}
