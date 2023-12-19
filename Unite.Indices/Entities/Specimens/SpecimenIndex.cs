namespace Unite.Indices.Entities.Specimens;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    /// <summary>
    /// Donor Id.
    /// </summary> 
    public int DonorId { get; set; }

    /// <summary>
    /// Parent specimen Id.
    /// </summary> 
    public int? ParentId { get; set; }

    /// <summary>
    /// Parent specimen reference Id.
    /// </summary> 
    public string ParentReferenceId { get; set; }

    /// <summary>
    /// Parent specimen type.
    /// </summary>
    public string ParentType { get; set; }


    /// <summary>
    /// Number of genes with at least one SSM, CNV or SV in all samples of the specimen.
    /// </summary>
    public int NumberOfGenes { get; set; }

    /// <summary>
    /// Number of SSMs in all samples of the specimen.
    /// </summary>
    public int NumberOfSsms { get; set; }

    /// <summary>
    /// Number of CNVs in all samples of the specimen.
    /// </summary>
    public int NumberOfCnvs { get; set; }

    /// <summary>
    /// Number of SVs in all samples of the specimen.
    /// </summary>
    public int NumberOfSvs { get; set; }


    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public DonorIndex Donor { get; set; }
    public AnalysisIndex[] Analyses { get; set; }
    public ImageIndex[] Images { get; set; }
}
