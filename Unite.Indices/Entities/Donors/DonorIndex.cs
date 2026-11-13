namespace Unite.Indices.Entities.Donors;

[SearchableDocument("donors", Key = nameof(DonorIndex.Id), Level = 1)]
public class DonorIndex : Basic.Donors.DonorIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public ImageIndex[] Images { get; set; }
    [References(typeof(Specimens.SpecimenIndex))]
    public SpecimenIndex[] Specimens { get; set; }
}
