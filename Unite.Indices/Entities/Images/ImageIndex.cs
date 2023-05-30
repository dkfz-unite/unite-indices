namespace Unite.Indices.Entities.Images;

public class ImageIndex : Basic.Images.ImageIndex
{
    /// <summary>
    /// Number of genes with at least one SSM, CNV or SV in all samples associated with the image.
    /// </summary>
    public int NumberOfGenes { get; set; }

    /// <summary>
    /// Number of SSMs in all samples associated with the image.
    /// </summary>
    public int NumberOfSSMs { get; set; }

    /// <summary>
    /// Number of CNVs in all samples associated with the image.
    /// </summary>
    public int NumberOfCNVs { get; set; }

    /// <summary>
    /// Number of SVs in all samples associated with the image.
    /// </summary>
    public int NumberOfSVs { get; set; }


    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public DonorIndex Donor { get; set; }
    public SpecimenIndex[] Specimens { get; set; }
}
