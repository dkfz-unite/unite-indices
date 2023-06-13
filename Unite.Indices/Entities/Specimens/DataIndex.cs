namespace Unite.Indices.Entities.Specimens;

public class DataIndex : Basic.IImagesDataIndex, Basic.IGenomeDataIndex
{
    /// <summary>
    /// Whether the specimen donor has clinical data.
    /// </summary>
    public bool? Clinical { get; set; }

    /// <summary>
    /// Whether the specimen donor has treatments data.
    /// </summary>
    public bool? Treatments { get; set; }

    /// <summary>
    /// Whether the specimen has molecular data.
    /// </summary>
    public bool? Molecular { get; set; }

    /// <summary>
    /// Whether the specimen has drugs screening data.
    /// </summary>
    public bool? Drugs { get; set; }

    /// <summary>
    /// Whether the specimen has interventions data.
    /// </summary>
    public bool? Interventions { get; set; }

    /// <summary>
    /// Whether the tumor tissue donor has MRI images.
    /// </summary>
    public bool? Mris { get; set; }

    /// <summary>
    /// Whether the tumor tissue donor or xenograft has CT images.
    /// </summary>
    public bool? Cts { get; set; }

    /// <summary>
    /// Whether any sample of the specimen has simple somatic mutations (SSM) data.
    /// </summary>
    public bool? Ssms { get; set; }

    /// <summary>
    /// Whether any sample of the specimen has copy number variants (CNV) data.
    /// </summary>
    public bool? Cnvs { get; set; }

    /// <summary>
    /// Whether any sample of the specimen has structural variants (SV) data.
    /// </summary>
    public bool? Svs { get; set; }

    /// <summary>
    /// Whether any sample of the specimen has bulk gene expression data.
    /// </summary>
    public bool? GeneExp { get; set; }

    /// <summary>
    /// Whether any sample of the specimen has single cell gene expression data.
    /// </summary>
    public bool? GeneExpSc { get; set; }
}
