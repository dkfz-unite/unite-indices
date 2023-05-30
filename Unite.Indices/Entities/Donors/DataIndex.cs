namespace Unite.Indices.Entities.Donors;

public class DataIndex : Basic.IImagesDataIndex, Basic.ISpecimensDataIndex, Basic.IGenomeDataIndex
{
    public bool MRIs { get; set; }
    public bool CTs { get; set; }
    public bool Tissues { get; set; }
    public bool Cells { get; set; }
    public bool Organoids { get; set; }
    public bool Xenografts { get; set; }
    public bool SSMs { get; set; }
    public bool CNVs { get; set; }
    public bool SVs { get; set; }
    public bool GeneExp { get; set; }
    public bool GeneExpSc { get; set; }
}
