namespace Unite.Indices.Entities.Specimens;

[SearchableDocument("specimens")]
public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public ParentIndex Parent { get; set; }
    [References(typeof(Donors.DonorIndex))]
    public DonorIndex Donor { get; set; }
    public ImageIndex[] Images { get; set; }
    public SampleIndex[] Samples { get; set; }
}
