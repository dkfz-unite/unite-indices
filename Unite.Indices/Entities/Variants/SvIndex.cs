namespace Unite.Indices.Entities.Variants;

[SearchableDocument("svs")]
[SearchOrder("stats.donors", Descending = true, Priority = 1)]
[SearchOrder("chromosomeI", Priority = 2)]
[SearchOrder("start", Priority = 3)]
public class SvIndex : Basic.Omics.Variants.SvIndex
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
    
    [ExcludedFromGet]
    [References(typeof(Specimens.SpecimenIndex))]
    public SpecimenIndex[] Specimens { get; set; }
}
