namespace Unite.Indices.Entities.Proteins;

[SearchableDocument("proteins")]
[SearchOrder("stats.donors", Descending = true)]
public class ProteinIndex : Basic.Omics.ProteinIndex
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


    [References(typeof(Genes.GeneIndex))]
    public GeneIndex Gene { get; set; }
    public TranscriptIndex Transcript { get; set; }
    [ExcludedFromGet]
    [References(typeof(Specimens.SpecimenIndex))]
    public SpecimenIndex[] Specimens { get; set; }
}
