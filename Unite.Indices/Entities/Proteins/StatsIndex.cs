namespace Unite.Indices.Entities.Proteins;

public class StatsIndex
{
    /// <summary>
    /// Number of donors with at least one SM, CNV or SV in this protein.
    /// </summary>
    public int Donors { get; set; }

    /// <summary>
    /// Number of SMs in this protein.
    /// </summary>
    public int Sms { get; set; }

    /// <summary>
    /// Number of CNVs in this protein.
    /// </summary>
    public int Cnvs { get; set; }

    /// <summary>
    /// Number of SVs in this protein.
    /// </summary>
    public int Svs { get; set; }
}
