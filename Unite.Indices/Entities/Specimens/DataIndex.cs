namespace Unite.Indices.Entities.Specimens;

public class DataIndex : Basic.IImagesDataIndex, Basic.IGenomeDataIndex
{
    /// <summary>
    /// Always true.
    /// </summary>
    public bool? Donors { get; set; } = true;

    /// <summary>
    /// Whether the specimen donor has clinical data.
    /// </summary>
    public bool? Clinical { get; set; }

    /// <summary>
    /// Whether the specimen donor has treatments data.
    /// </summary>
    public bool? Treatments { get; set; }

    /// <summary>
    /// Whether the specimen is tissue.
    /// </summary>
    public bool? Tissues { get; set; }

    /// <summary>
    /// Whether the specimen is cell line.
    /// </summary>
    public bool? Cells { get; set; }

    /// <summary>
    /// Whether the specimen is organoid.
    /// </summary>
    public bool? Organoids { get; set; }

    /// <summary>
    /// Whether the specimen is xenograft.
    /// </summary>
    public bool? Xenografts { get; set; }

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
    /// Whether the specimen is associated with any MRI image.
    /// </summary>
    public bool? Mris { get; set; }

    /// <summary>
    /// Whether the specimen is associated with any CT image.
    /// </summary>
    public bool? Cts { get; set; }

    /// <summary>
    /// Whether any sample of the specimen has SSMs.
    /// </summary>
    public bool? Ssms { get; set; }

    /// <summary>
    /// Whether any sample of the specimen has CNVs.
    /// </summary>
    public bool? Cnvs { get; set; }

    /// <summary>
    /// Whether any sample of the specimen has SVs.
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
