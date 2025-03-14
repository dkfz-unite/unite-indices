namespace Unite.Indices.Entities.Projects.Stats;

public class RnascStats
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
    /// Breakdown of donors per cells number.
    /// </summary>
    public Dictionary<string, int> PerCells { get; set; }
}
