namespace Unite.Indices.Entities.Variants;

public class DataIndex : Basic.IDonorsDataIndex, Basic.IImagesDataIndex, Basic.ISpecimensDataIndex, Basic.IGenomeDataIndex
{
    /// <summary>
    /// Always true.
    /// </summary>
    public bool? Donors { get; set; } = true;

    /// <summary>
    /// Whether any donor having this variant has clinical data.
    /// </summary>
    public bool? Clinical { get; set; }

    /// <summary>
    /// Whether any donor having this variant has treatments data.
    /// </summary>
    public bool? Treatments { get; set; }

    /// <summary>
    /// Whether any MRI image associated specimen has this variant.
    /// </summary>
    public bool? Mris { get; set; }

    /// <summary>
    /// Whether any CT image associated specimen has this variant.
    /// </summary>
    public bool? Cts { get; set; }

    /// <summary>
    /// Whether any tissue has this variant.
    /// </summary>
    public bool? Tissues { get; set; }

    /// <summary>
    /// Whether any tissue having this variant has molecular data.
    /// </summary>
    public bool? TissuesMolecular { get; set; }

    /// <summary>
    /// Whether any cell line has this variant.
    /// </summary>
    public bool? Cells { get; set; }

    /// <summary>
    /// Whether any cell line having this variant has molecular data.
    /// </summary>
    public bool? CellsMolecular { get; set; }

    /// <summary>
    /// Whether any cell line having this variant has drugs screening data.
    /// </summary>
    public bool? CellsDrugs { get; set; }

    /// <summary>
    /// Whether any organoid has this variant.
    /// </summary>
    public bool? Organoids { get; set; }

    /// <summary>
    /// Whether any organoid having this variant has molecular data.
    /// </summary>
    public bool? OrganoidsMolecular { get; set; }

    /// <summary>
    /// Whether any organoid having this variant has drugs screening data.
    /// </summary>
    public bool? OrganoidsDrugs { get; set; }

    /// <summary>
    /// Whether any organoid having this variant has interventions data.
    /// </summary>
    public bool? OrganoidsInterventions { get; set; }

    /// <summary>
    /// Whether any xenograft has this variant.
    /// </summary>
    public bool? Xenografts { get; set; }

    /// <summary>
    /// Whether any xenograft having this variant has molecular data.
    /// </summary>
    public bool? XenograftsMolecular { get; set; }

    /// <summary>
    /// Whether any xenograft having this variant has drugs screening data.
    /// </summary>
    public bool? XenograftsDrugs { get; set; }

    /// <summary>
    /// Whether any xenograft having this variant has interventions data.
    /// </summary>
    public bool? XenograftsInterventions { get; set; }

    /// <summary>
    /// Whether this variant overlaps with any SSM or it's an SSM itself.
    /// </summary>
    public bool? Ssms { get; set; }

    /// <summary>
    /// Whether this variant overlaps with any CNV or it's a CNV itself.
    /// </summary>
    public bool? Cnvs { get; set; }

    /// <summary>
    /// Whether this variant overlaps with any SV or it's an SV itself.
    /// </summary>
    public bool? Svs { get; set; }

    /// <summary>
    /// Whether any gene affected by this variant has bulk gene expression data.
    public bool? GeneExp { get; set; }

    /// <summary>
    /// Whether any gene affected by this variant has single cell gene expression data.
    /// </summary>
    public bool? GeneExpSc { get; set; }
}
