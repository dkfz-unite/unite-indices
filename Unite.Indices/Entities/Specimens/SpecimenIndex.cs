namespace Unite.Indices.Entities.Specimens;

public class SpecimenIndex : Basic.Specimens.SpecimenIndex
{
    /// <summary>
    /// Number of genes with at least one SSM, CNV or SV in all samples of the specimen.
    /// </summary>
    public int NumberOfGenes { get; set; }

    /// <summary>
    /// Number of SSMs in all samples of the specimen.
    /// </summary>
    public int NumberOfSSMs { get; set; }

    /// <summary>
    /// Number of CNVs in all samples of the specimen.
    /// </summary>
    public int NumberOfCNVs { get; set; }

    /// <summary>
    /// Number of SVs in all samples of the specimen.
    /// </summary>
    public int NumberOfSVs { get; set; }


    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public SpecimenIndex Parent { get; set; }
    public DonorIndex Donor { get; set; }
    public ImageIndex[] Images { get; set; }
    public SampleIndex[] Samples { get; set; }
}
