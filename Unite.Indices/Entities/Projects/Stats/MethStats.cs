namespace Unite.Indices.Entities.Projects.Stats;

public class MethStats
{
    /// <summary>
    /// Donors with the data.
    /// </summary>
    public int Number { get; set; }


    /// <summary>
    /// Breakdown of donors per analysis type.
    /// </summary>
    public Dictionary<string, int> PerAnalysis { get; set; }
}
