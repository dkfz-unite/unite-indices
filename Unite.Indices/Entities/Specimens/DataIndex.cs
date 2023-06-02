namespace Unite.Indices.Entities.Specimens;

public class DataIndex : Basic.IImagesDataIndex, Basic.IGenomeDataIndex
{
    public bool Molecular { get; set; }
    public bool Mris { get; set; }
    public bool Cts { get; set; }
    public bool Ssms { get; set; }
    public bool Cnvs { get; set; }
    public bool Svs { get; set; }
    public bool GeneExp { get; set; }
    public bool GeneExpSc { get; set; }
    public bool Drugs { get; set; }
    public bool Interventions { get; set; }
}
