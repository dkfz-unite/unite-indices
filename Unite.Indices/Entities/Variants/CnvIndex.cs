namespace Unite.Indices.Entities.Variants;

public class CnvIndex : Basic.Omics.Variants.CnvIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public Basic.Omics.Variants.VariantNavIndex[] Similars { get; set; }

    public SpecimenIndex[] Specimens { get; set; }
}
