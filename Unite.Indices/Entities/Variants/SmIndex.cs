namespace Unite.Indices.Entities.Variants;

public class SmIndex : Basic.Omics.Variants.SmIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public SpecimenIndex[] Specimens { get; set; }
}
