namespace Unite.Indices.Entities.Donors;

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
    public SpecimenIndex[] Specimens { get; set; }
}
