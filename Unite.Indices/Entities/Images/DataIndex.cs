namespace Unite.Indices.Entities.Images;

public class DataIndex : Basic.IGenomeDataIndex
{
    /// <summary>
    /// Whether the image donor has clinical data.
    /// </summary>
    public bool? Clinical { get; set; }

    /// <summary>
    /// Whether the image donor has treatments data.
    /// </summary>
    public bool? Treatments { get; set; }

    /// <summary>
    /// Whether the image donor has tumor tissues.
    /// </summary>
    public bool? Tissues { get; set; }

    /// <summary>
    /// Whether the image donor tumor tissues have molecular data.
    /// </summary>
    public bool? TissuesMolecular { get; set; }

    /// <summary>
    /// Whether any sample of image donor tumor tissues has simple somatic mutations (SSM) data.
    /// </summary>
    public bool? Ssms { get; set; }

    /// <summary>
    /// Whether any sample of image donor tumor tissues has copy number variants (CNV) data.
    /// </summary>
    public bool? Cnvs { get; set; }

    /// <summary>
    /// Whether any sample of image donor tumor tissues has structural variants (SV) data.
    /// </summary>
    public bool? Svs { get; set; }

    /// <summary>
    /// Whether any sample of image donor tumor tissues has bulk gene expression data.
    /// </summary>
    public bool? GeneExp { get; set; }

    /// <summary>
    /// Whether any sample of image donor tumor tissues has single cell gene expression data.
    /// </summary>
    public bool? GeneExpSc { get; set; }
}
