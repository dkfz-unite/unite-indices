namespace Unite.Indices.Entities.Proteins;

public class ProteinIndex : Basic.Omics.ProteinIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public GeneIndex Gene { get; set; }
    public TranscriptIndex Transcript { get; set; }
    public SpecimenIndex[] Specimens { get; set; }
}
