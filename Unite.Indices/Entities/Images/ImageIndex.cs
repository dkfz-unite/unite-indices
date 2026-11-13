namespace Unite.Indices.Entities.Images;

[SearchableDocument("images")]
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


    [Searchable]
    public DonorIndex Donor { get; set; }
    [Searchable]
    public SpecimenIndex[] Specimens { get; set; }
}
