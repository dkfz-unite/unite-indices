namespace Unite.Indices.Entities.Genes;

public class StatsIndex
{
    /// <summary>
    /// Number of donors with at least one SSM, CNV or SV in this gene.
    /// </summary>
    public int Donors { get; set; }

    /// <summary>
    /// Number of SSMs in this gene.
    /// </summary>
    public int Ssms { get; set; }

    /// <summary>
    /// Number of CNVs in this gene.
    /// </summary>
    public int Cnvs { get; set; }

    /// <summary>
    /// Number of SVs in this gene.
    /// </summary>
    public int Svs { get; set; }
}
