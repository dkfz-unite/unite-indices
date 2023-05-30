namespace Unite.Indices.Entities.Specimens;

public class DataIndex : Basic.IImagesDataIndex, Basic.IGenomeDataIndex
{
    public bool MRIs { get; set; }
    public bool CTs { get; set; }
    public bool SSMs { get; set; }
    public bool CNVs { get; set; }
    public bool SVs { get; set; }
    public bool GeneExp { get; set; }
    public bool GeneExpSc { get; set; }
    public bool Drugs { get; set; }
}
