namespace Unite.Indices.Entities.Projects.Stats;

public class DonorsStats
{
    /// <summary>
    /// Total number of donors.
    /// </summary>
    public int Number { get; set; }


    /// <summary>
    /// Breakdown per age.
    /// </summary>
    public Dictionary<string, int> PerAge { get; set; }

    /// <summary>
    /// Breakdown per sex.
    /// </summary>
    public Dictionary<string, int> PerGender { get; set; }

    /// <summary>
    /// Breakdown per diagnosis.
    /// </summary>
    public Dictionary<string, int> PerDiagnosis { get; set; }

    /// <summary>
    /// Breakdown per vital status.
    /// </summary>
    public Dictionary<string, int> PerVitalStatus { get; set; }

    /// <summary>
    /// Breakdown per progression status.
    /// </summary>
    public Dictionary<string, int> PerProgressionStatus { get; set; }
}
