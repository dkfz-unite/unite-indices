namespace Unite.Indices.Entities.Donors;

public class DonorIndex : Basic.Donors.DonorIndex
{
    /// <summary>
    /// Number of genes with at least one SSM, CNV or SV in all samples of the donor.
    /// </summary>
    public int NumberOfGenes { get; set; }

    /// <summary>
    /// Number of SSMs in all samples of the donor.
    /// </summary>
    public int NumberOfSSMs { get; set; }

    /// <summary>
    /// Number of CNVs in all samples of the donor.
    /// </summary>
    public int NumberOfCNVs { get; set; }

    /// <summary>
    /// Number of SVs in all samples of the donor.
    /// </summary>
    public int NumberOfSVs { get; set; }


    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public ImageIndex[] Images { get; set; }
    public SpecimenIndex[] Specimens { get; set; }
}
