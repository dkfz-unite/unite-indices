namespace Unite.Indices.Entities.Variants;

[SearchableDocument("sms")]
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


    [References(typeof(Specimens.SpecimenIndex))]
    public SpecimenIndex[] Specimens { get; set; }
}
