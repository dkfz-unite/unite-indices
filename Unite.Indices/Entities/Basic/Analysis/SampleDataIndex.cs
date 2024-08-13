namespace Unite.Indices.Entities.Basic.Analysis;

public class SampleDataIndex
{
    /// <summary>
    /// DNA Simple Somatic Mutations (SSM) data availability.
    /// </summary>
    public bool Ssm { get; set; }

    /// <summary>
    /// DNA Copy Number Variants (CNV) data availability.
    /// </summary>
    public bool Cnv { get; set; }

    /// <summary>
    /// DNA Structural Variants (SV) data availability.
    /// </summary> 
    public bool Sv { get; set; }

    /// <summary>
    /// Bulk RNA gene expressions data availability.
    public bool Exp { get; set; }
}
