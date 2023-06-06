namespace Unite.Indices.Entities.Donors;

public class DataIndex : Basic.IImagesDataIndex, Basic.ISpecimensDataIndex, Basic.IGenomeDataIndex
{
    public bool? Clinical { get; set; }
    public bool? Treatments { get; set; }
    public bool? Mris { get; set; }
    public bool? Cts { get; set; }
    public bool? Tissues { get; set; }
    public bool? Cells { get; set; }
    public bool? Organoids { get; set; }
    public bool? Xenografts { get; set; }
    public bool? Ssms { get; set; }
    public bool? Cnvs { get; set; }
    public bool? Svs { get; set; }
    public bool? GeneExp { get; set; }
    public bool? GeneExpSc { get; set; }
}
