namespace Unite.Indices.Entities.Images;

public class DataIndex : Basic.IGenomeDataIndex
{
    public bool SSMs { get; set; }
    public bool CNVs { get; set; }
    public bool SVs { get; set; }
    public bool GeneExp { get; set; }
    public bool GeneExpSc { get; set; }
}
