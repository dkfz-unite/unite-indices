namespace Unite.Indices.Entities.Images;

public class ImageIndex : Basic.Images.ImageIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public DonorIndex Donor { get; set; }
    public SpecimenIndex[] Specimens { get; set; }
}
