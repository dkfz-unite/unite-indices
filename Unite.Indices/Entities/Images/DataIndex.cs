namespace Unite.Indices.Entities.Images;

public class DataIndex : Basic.IDonorsDataIndex, Basic.IImagesDataIndex, Basic.IGenomeDataIndex
{
    /// <summary>
    /// Always true.
    /// </summary>
    public bool? Donors { get; set; } = true;

    /// <summary>
    /// Whether this image donor has clinical data.
    /// </summary>
    public bool? Clinical { get; set; }

     /// <summary>
    /// Whether this image donor has treatments data.
    /// </summary>
    public bool? Treatments { get; set; }

    /// <summary>
    /// Whether this image is MRI image.
    /// </summary>
    public bool? Mris { get; set; }

    /// <summary>
    /// Whether this image is CT image.
    /// </summary>
    public bool? Cts { get; set; }

    /// <summary>
    /// Whether this image has assosiated tissues.
    /// </summary>
    public bool? Tissues { get; set; }

    /// <summary>
    /// Whether this image assosiated tissues have molecular data.
    /// </summary>
    public bool? TissuesMolecular { get; set; }

    /// <summary>
    /// Whether this image has associated xenografts.
    /// </summary>
    public bool? Xenografts { get; set; }

    /// <summary>
    /// Whether this image associated xenografts have molecular data.
    /// </summary>
    public bool? XenograftsMolecular { get; set; }

    /// <summary>
    /// Whether this image associated xenografts have drugs screening data.
    /// </summary>
    public bool? XenograftsDrugs { get; set; }

    /// <summary>
    /// Whether this image associated xenografts have interventions data.
    /// </summary>
    public bool? XenograftsInterventions { get; set; }

    /// <summary>
    /// Whether this image has SSMs in any sample of associated specimens.
    /// </summary>
    public bool? Ssms { get; set; }

    /// <summary>
    /// Whether this image has CNVs in any sample of associated specimens.
    /// </summary>
    public bool? Cnvs { get; set; }

    /// <summary>
    /// Whether this image has SVs in any sample of associated specimens.
    /// </summary>
    public bool? Svs { get; set; }

    /// <summary>
    /// Whether this image has bulk gene expressions data in any sample of associated specimens.
    /// </summary>
    public bool? GeneExp { get; set; }

    /// <summary>
    /// Whether this image has single cell gene expressions data in any sample of associated specimens.
    /// </summary>
    public bool? GeneExpSc { get; set; }
}
