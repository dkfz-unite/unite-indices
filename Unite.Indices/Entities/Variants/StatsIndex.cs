namespace Unite.Indices.Entities.Variants;

public class StatsIndex
{
    /// <summary>
    /// Number of donors with the variant in any sample.
    /// </summary>
    public int Donors { get; set; }

    /// <summary>
    /// Number of genes affected by the variant in any sample.
    /// </summary>
    public int Genes { get; set; }
}
