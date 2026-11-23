namespace Unite.Indices.Entities.Genes;

[SearchableDocument("genes")]
[SearchOrder("stats.donors", Descending = true)]
public class GeneIndex : Basic.Omics.GeneIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    [Searchable]
    public DataIndex Data { get; set; }


    [ExcludedFromGet]
    [References(typeof(Specimens.SpecimenIndex))]
    public SpecimenIndex[] Specimens { get; set; }
}
