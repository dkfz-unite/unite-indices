namespace Unite.Indices.Entities.Genes;

public class DataIndex : Basic.IDonorsDataIndex, Basic.IImagesDataIndex, Basic.ISpecimensDataIndex, Basic.IGenomeDataIndex
{
    /// <summary>
    /// Always true.
    /// </summary>
    public bool? Donors { get; set; } = true;

    /// <summary>
    /// Whether any donor having this gene mutated or expressed has clinical data.
    /// </summary>
    public bool? Clinical { get; set; }

    /// <summary>
    /// Whether any donor having this gene mutated or expressed has treatments data.
    /// </summary>
    public bool? Treatments { get; set; }

    /// <summary>
    /// Whether any donor having this gene mutated or expressed has MRI image associated specimens.
    /// </summary>
    public bool? Mris { get; set; }

    /// <summary>
    /// Whether any donor having this gene mutated or expressed CT image associated specimens.
    /// </summary>
    public bool? Cts { get; set; }

    /// <summary>
    /// Whether any tissue has this gene mutated or expressed.
    /// </summary>
    public bool? Tissues { get; set; }

    /// <summary>
    /// Whether any tissue having this gene mutated or expressed has molecular data.
    /// </summary>
    public bool? TissuesMolecular { get; set; }

    /// <summary>
    /// Whether any cell line has this gene mutated or expressed.
    /// </summary>
    public bool? Cells { get; set; }

    /// <summary>
    /// Whether any cell line having this gene mutated or expressed has molecular data.
    /// </summary>
    public bool? CellsMolecular { get; set; }

    /// <summary>
    /// Whether any cell line having this gene mutated or expressed has drugs screening data.
    /// </summary>
    public bool? CellsDrugs { get; set; }

    /// <summary>
    /// Whether any organoid has this gene mutated or expressed.
    /// </summary>
    public bool? Organoids { get; set; }

    /// <summary>
    /// Whether any organoid having this gene mutated or expressed has molecular data.
    /// </summary>
    public bool? OrganoidsMolecular { get; set; }

    /// <summary>
    /// Whether any organoid having this gene mutated or expressed has drugs screening data.
    /// </summary>
    public bool? OrganoidsDrugs { get; set; }

    /// <summary>
    /// Whether any organoid having this gene mutated or expressed has interventions data.
    /// </summary>
    public bool? OrganoidsInterventions { get; set; }

    /// <summary>
    /// Whether any xenograft has this gene mutated or expressed.
    /// </summary>
    public bool? Xenografts { get; set; }

    /// <summary>
    /// Whether any xenograft having this gene mutated or expressed has molecular data.
    /// </summary>
    public bool? XenograftsMolecular { get; set; }

    /// <summary>
    /// Whether any xenograft having this gene mutated or expressed has drugs screening data.
    /// </summary>
    public bool? XenograftsDrugs { get; set; }

    /// <summary>
    /// Whether any xenograft having this gene mutated or expressed has interventions data.
    /// </summary>
    public bool? XenograftsInterventions { get; set; }

    /// <summary>
    /// Whether any SSM affects this gene.
    /// </summary>
    public bool? Ssms { get; set; }

    /// <summary>
    /// Whether any CNV affects this gene.
    /// </summary>
    public bool? Cnvs { get; set; }

    /// <summary>
    /// Whether any SV affects this gene.
    /// </summary>
    public bool? Svs { get; set; }

    /// <summary>
    /// Whether this gene has bulk gene expression data in any sample.
    /// </summary>
    public bool? GeneExp { get; set; }

    /// <summary>
    /// Whether this gene has single cell gene expression data in any sample.
    /// </summary>
    public bool? GeneExpSc { get; set; }
}
