namespace Unite.Indices.Entities.Donors;

public class DataIndex : Basic.IDonorsDataIndex, Basic.IImagesDataIndex, Basic.ISpecimensDataIndex, Basic.IGenomeDataIndex
{
    /// <summary>
    /// Always true.
    /// </summary>
    public bool? Donors { get; set; } = true;

    /// <summary>
    /// Whether this donor has clinical data.
    /// </summary>
    public bool? Clinical { get; set; }

    /// <summary>
    /// Whether this donor has treatmens data.
    /// </summary>
    public bool? Treatments { get; set; }

    /// <summary>
    /// Whether this donor has MRI images.
    /// </summary>
    public bool? Mris { get; set; }

    /// <summary>
    /// Whether this donor has CT images.
    /// </summary>
    public bool? Cts { get; set; }

    /// <summary>
    /// Whether this donor has tissues.
    /// </summary>
    public bool? Tissues { get; set; }

    /// <summary>
    /// Whether this donor tissues have molecular data.
    /// </summary>
    public bool? TissuesMolecular { get; set; }

    /// <summary>
    /// Whether this donor has cell lines.
    /// </summary>
    public bool? Cells { get; set; }

    /// <summary>
    /// Whether this donor cell lines have molecular data.
    /// </summary>
    public bool? CellsMolecular { get; set; }

    /// <summary>
    /// Whether this donor cell lines have drugs screening data.
    /// </summary>
    public bool? CellsDrugs { get; set; }

    /// <summary>
    /// Whether this donor has organoids.
    /// </summary>
    public bool? Organoids { get; set; }

    /// <summary>
    /// Whether this donor organoids have molecular data.
    /// </summary>
    public bool? OrganoidsMolecular { get; set; }

    /// <summary>
    /// Whether this donor organoids have drugs screening data.
    /// </summary>
    public bool? OrganoidsDrugs { get; set; }

    /// <summary>
    /// Whether this donor organoids have interventions data.
    /// </summary>
    public bool? OrganoidsInterventions { get; set; }

    /// <summary>
    /// Whether this donor has xenografts.
    /// </summary>
    public bool? Xenografts { get; set; }

    /// <summary>
    /// Whether this donor xenografts have molecular data.
    /// </summary>
    public bool? XenograftsMolecular { get; set; }

    /// <summary>
    /// Whether this donor xenografts have drugs screening data.
    /// </summary>
    public bool? XenograftsDrugs { get; set; }

    /// <summary>
    /// Whether this donor xenografts have interventions data.
    /// </summary>
    public bool? XenograftsInterventions { get; set; }

    /// <summary>
    /// Whether any sample of this donor has SSMs.
    /// </summary>
    public bool? Ssms { get; set; }

    /// <summary>
    /// Whether any sample of this donor has CNVs.
    /// </summary>
    public bool? Cnvs { get; set; }

    /// <summary>
    /// Whether any sample of this donor has SVs.
    /// </summary>
    public bool? Svs { get; set; }

    /// <summary>
    /// Whether any sample of this donor has bulk gene expression data.
    /// </summary>
    public bool? GeneExp { get; set; }

    /// <summary>
    /// Whether any sample of this donor has single cell gene expression data.
    /// </summary>
    public bool? GeneExpSc { get; set; }
}
