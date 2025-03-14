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
    public Dictionary<string, int> PerAnalysis { get; set; }

    /// <summary>
    /// Breakdown of top most variable genes per their min and max expression.
    /// </summary>
    public Dictionary<string, int[]> PerVariation { get; set; }

    /// <summary>
    /// Breakdown of top most mutated genes per their mutations count.
    /// </summary>
    public Dictionary<string, int> PerMutation { get; set; }
}
