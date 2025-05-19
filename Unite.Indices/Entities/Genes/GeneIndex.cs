namespace Unite.Indices.Entities.Genes;

public class GeneIndex : Basic.Omics.GeneIndex
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
