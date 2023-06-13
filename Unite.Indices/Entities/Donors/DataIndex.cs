namespace Unite.Indices.Entities.Donors;

public class DataIndex : Basic.IImagesDataIndex, Basic.ISpecimensDataIndex, Basic.IGenomeDataIndex
{
    /// <summary>
    /// Whether the donor has clinical data.
    /// </summary>
    public bool? Clinical { get; set; }

    /// <summary>
    /// Whether the donor has treatment data.
    /// </summary>
    public bool? Treatments { get; set; }

    /// <summary>
    /// Whether the donor has MRI images.
    /// </summary>
    public bool? Mris { get; set; }

    /// <summary>
    /// Whether the donor has CT images.
    /// </summary>
    public bool? Cts { get; set; }

    /// <summary>
    /// Whether the donor has tissues.
    /// </summary>
    public bool? Tissues { get; set; }

    /// <summary>
    /// Whether the donor tissues have molecular data.
    /// </summary>
    public bool? TissuesMolecular { get; set; }

    /// <summary>
    /// Whether the donor has cell lines.
    /// </summary>
    public bool? Cells { get; set; }

    /// <summary>
    /// Whether the donor cell lines have molecular data.
    /// </summary>
    public bool? CellsMolecular { get; set; }

    /// <summary>
    /// Whether the donor cell lines have drug screening data.
    /// </summary>
    public bool? CellsDrugs { get; set; }

    /// <summary>
    /// Whether the donor has organoids.
    /// </summary>
    public bool? Organoids { get; set; }

    /// <summary>
    /// Whether the donor organoids have molecular data.
    /// </summary>
    public bool? OrganoidsMolecular { get; set; }

    /// <summary>
    /// Whether the donor organoids have drug screening data.
    /// </summary>
    public bool? OrganoidsDrugs { get; set; }

    /// <summary>
    /// Whether the donor organoids have interventions data.
    /// </summary>
    public bool? OrganoidsInterventions { get; set; }

    /// <summary>
    /// Whether the donor has xenografts.
    /// </summary>
    public bool? Xenografts { get; set; }

    /// <summary>
    /// Whether the donor xenografts have molecular data.
    /// </summary>
    public bool? XenograftsMolecular { get; set; }

    /// <summary>
    /// Whether the donor xenografts have drug screening data.
    /// </summary>
    public bool? XenograftsDrugs { get; set; }

    /// <summary>
    /// Whether the donor xenografts have interventions data.
    /// </summary>
    public bool? XenograftsInterventions { get; set; }

    /// <summary>
    /// Whether any sample of the donor derived specimens has simple somatic mutations (SSM) data.
    /// </summary>
    public bool? Ssms { get; set; }

    /// <summary>
    /// Whether any sample of the donor derived specimens has copy number variants (CNV) data.
    /// </summary>
    public bool? Cnvs { get; set; }

    /// <summary>
    /// Whether any sample of the donor derived specimens has structural variants (SV) data.
    /// </summary>
    public bool? Svs { get; set; }

    /// <summary>
    /// Whether any sample of the donor derived specimens has bulk gene expression data.
    /// </summary>
    public bool? GeneExp { get; set; }

    /// <summary>
    /// Whether any sample of the donor derived specimens has single cell gene expression data.
    /// </summary>
    public bool? GeneExpSc { get; set; }
}
