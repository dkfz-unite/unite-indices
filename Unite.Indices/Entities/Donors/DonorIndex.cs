namespace Unite.Indices.Entities.Donors;

[SearchableDocument("donors")]
[SearchOrder("stats.genes", Descending = true)]
public class DonorIndex : Basic.Donors.DonorIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    [Searchable]
    public DataIndex Data { get; set; }


    public ImageIndex[] Images { get; set; }
    [References(typeof(Specimens.SpecimenIndex))]
    public SpecimenIndex[] Specimens { get; set; }
}
