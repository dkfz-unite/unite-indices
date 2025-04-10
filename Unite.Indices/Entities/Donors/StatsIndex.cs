namespace Unite.Indices.Entities.Donors;

public class StatsIndex
{
    /// <summary>
    /// Number of genes with at least one SM, CNV or SV in all samples of the donor.
    /// </summary>
    public int Genes { get; set; }

    /// <summary>
    /// Number of SMs in all samples of the donor.
    /// </summary>
    public int Sms { get; set; }

    /// <summary>
    /// Number of CNVs in all samples of the donor.
    /// </summary>
    public int Cnvs { get; set; }

    /// <summary>
    /// Number of SVs in all samples of the donor.
    /// </summary>
    public int Svs { get; set; }
}
