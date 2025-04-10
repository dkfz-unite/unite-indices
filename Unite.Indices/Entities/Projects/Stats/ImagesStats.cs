using Unite.Indices.Entities.Projects.Stats.Base;

namespace Unite.Indices.Entities.Projects.Stats;

public class ImagesStats
{
    /// <summary>
    /// MR images data.
    /// </summary>
    public MrStats Mr { get; set; } = new();

    /// <summary>
    /// CT images data.
    /// </summary>
    public CtStats Ct { get; set; } = new();


    /// <summary>
    /// Breakdown per image type.
    /// </summary>
    public Stat<string, int>[] PerType { get; set; }
}

public class MrStats
{
    /// <summary>
    /// Donors with the data / total.
    /// </summary>
    public int[] Number { get; set; }


    /// <summary>
    /// Breakdown per size.
    /// </summary>
    public Stat<double?, int>[] PerSize { get; set; }
}

public class CtStats
{
    /// <summary>
    /// Donors with the data / total.
    /// </summary>
    public int[] Number { get; set; }
}
