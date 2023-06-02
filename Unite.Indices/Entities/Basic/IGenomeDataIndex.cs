namespace Unite.Indices.Entities.Basic;

public interface IGenomeDataIndex
{
    bool Ssms { get; set; }
    bool Cnvs { get; set; }
    bool Svs { get; set; }

    bool GeneExp { get; set; }
    bool GeneExpSc { get; set; }
}
