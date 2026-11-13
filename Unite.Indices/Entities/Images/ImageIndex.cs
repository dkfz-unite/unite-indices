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
    [References(typeof(Donors.DonorIndex))]
    public DonorIndex Donor { get; set; }
    [Searchable]
    [References(typeof(Specimens.SpecimenIndex))]
    public SpecimenIndex[] Specimens { get; set; }
}
