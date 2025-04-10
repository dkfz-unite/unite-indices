namespace Unite.Indices.Entities.Basic.Analysis;

public class SampleDataIndex
{
    /// <summary>
    /// DNA Simple Mutations (SM) data availability.
    /// </summary>
    public bool Sm { get; set; }

    /// <summary>
    /// DNA Copy Number Variants (CNV) data availability.
    /// </summary>
    public bool Cnv { get; set; }

    /// <summary>
    /// DNA Structural Variants (SV) data availability.
    /// </summary> 
    public bool Sv { get; set; }

    /// <summary>
    /// DNA Methylation data availability.
    /// </summary>
    public bool Meth { get; set; }

    /// <summary>
    /// Bulk RNA gene expressions data availability.
    /// </summary>
    public bool Exp { get; set; }

    /// <summary>
    /// Single-cell RNA gene expressions data availability.
    /// </summary>
    public bool ExpSc { get; set; }
}
