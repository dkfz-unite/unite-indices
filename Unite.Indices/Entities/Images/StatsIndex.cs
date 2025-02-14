namespace Unite.Indices.Entities.Images;

public class StatsIndex
{
    /// <summary>
    /// Number of genes with at least one SSM, CNV or SV in all samples associated with the image.
    /// </summary>
    public int Genes { get; set; }

    /// <summary>
    /// Number of SSMs in all samples associated with the image.
    /// </summary>
    public int Ssms { get; set; }

    /// <summary>
    /// Number of CNVs in all samples associated with the image.
    /// </summary>
    public int Cnvs { get; set; }

    /// <summary>
    /// Number of SVs in all samples associated with the image.
    /// </summary>
    public int Svs { get; set; }
}
