using Unite.Indices.Entities.Projects.Stats.Base;

namespace Unite.Indices.Entities.Projects.Stats;

public class ProtStats
{
    /// <summary>
    /// Donors / specimens with the data.
    /// </summary>
    public int[] Number { get; set; }


    /// <summary>
    /// Breakdown of donors per analysis type.
    /// </summary>
    public Stat<string, int>[] PerAnalysis { get; set; }

    /// <summary>
    /// Breakdown of top most variable proteins, where Key is the protein name and Value is the array of stats for the box plot:
    /// [min, 25th percentile, 50th percentile, 75th percentile, max, mean, standard deviation (SD), standard deviation / mean (CV)].
    /// </summary>
    public Stat<string, double[]>[] PerVariation { get; set; }

    /// <summary>
    /// Breakdown of top most mutated proteins per their mutations count.
    /// </summary>
    public Stat<string, int>[] PerMutation { get; set; }
}
