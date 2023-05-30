namespace Unite.Indices.Entities.Basic;

public interface IGenomeDataIndex
{
    bool SSMs { get; set; }
    bool CNVs { get; set; }
    bool SVs { get; set; }

    bool GeneExp { get; set; }
    bool GeneExpSc { get; set; }
}
