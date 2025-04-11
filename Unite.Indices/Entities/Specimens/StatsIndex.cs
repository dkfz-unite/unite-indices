namespace Unite.Indices.Entities.Specimens;

public class StatsIndex
{
    /// <summary>
    /// Number of genes with at least one SM, CNV or SV in all samples of the specimen.
    /// </summary>
    public int Genes { get; set; }

    /// <summary>
    /// Number of SMs in all samples of the specimen.
    /// </summary>
    public int Sms { get; set; }

    /// <summary>
    /// Number of CNVs in all samples of the specimen.
    /// </summary>
    public int Cnvs { get; set; }

    /// <summary>
    /// Number of SVs in all samples of the specimen.
    /// </summary>
    public int Svs { get; set; }
}
