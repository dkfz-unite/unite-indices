namespace Unite.Indices.Entities.Variants;

public class SvIndex : Basic.Genome.Variants.SvIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public Basic.Genome.Variants.VariantNavIndex[] Similars { get; set; }
    
    public SpecimenIndex[] Specimens { get; set; }
}
