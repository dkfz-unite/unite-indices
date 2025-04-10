namespace Unite.Indices.Entities.Genes;

public class StatsIndex
{
    /// <summary>
    /// Number of donors with at least one SM, CNV or SV in this gene.
    /// </summary>
    public int Donors { get; set; }

    /// <summary>
    /// Number of SMs in this gene.
    /// </summary>
    public int Sms { get; set; }

    /// <summary>
    /// Number of CNVs in this gene.
    /// </summary>
    public int Cnvs { get; set; }

    /// <summary>
    /// Number of SVs in this gene.
    /// </summary>
    public int Svs { get; set; }
}
