using Unite.Indices.Entities.Projects.Stats.Base;

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
    public Stat<int?, int>[] PerAge { get; set; }

    /// <summary>
    /// Breakdown per sex.
    /// </summary>
    public Stat<string, int>[] PerSex { get; set; }

    /// <summary>
    /// Breakdown per diagnosis.
    /// </summary>
    public Stat<string, int>[] PerDiagnosis { get; set; }

    /// <summary>
    /// Breakdown per vital status.
    /// </summary>
    public Stat<bool?, int>[] PerVitalStatus { get; set; }

    /// <summary>
    /// Breakdown per progression status.
    /// </summary>
    public Stat<bool?, int>[] PerProgressionStatus { get; set; }
}
