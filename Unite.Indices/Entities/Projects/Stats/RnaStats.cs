using Unite.Indices.Entities.Projects.Stats.Base;

namespace Unite.Indices.Entities.Projects.Stats;

public class RnaStats
{
    /// <summary>
    /// Donors with the data.
    /// </summary>
    public int Number { get; set; }


    /// <summary>
    /// Breakdown of donors per analysis type.
    /// </summary>
    public Stat<string, int>[] PerAnalysis { get; set; }

    /// <summary>
    /// Breakdown of top most variable genes, where Key is the gene name and Value is the array of stats for the box plot:
    /// [min, 25th percentile, median, 75th percentile, max, mean, standard deviation, standard deviation / mean].
    /// </summary>
    public Stat<string, double[]>[] PerVariation { get; set; }

    /// <summary>
    /// Breakdown of top most mutated genes per their mutations count.
    /// </summary>
    public Stat<string, int>[] PerMutation { get; set; }
}
