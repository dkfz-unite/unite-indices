namespace Unite.Indices.Entities.Projects.Stats;

public class ImagesStats
{
    /// <summary>
    /// MRI images data.
    /// </summary>
    public MriStats Mri { get; set; } = new();

    /// <summary>
    /// CT images data.
    /// </summary>
    public CtStats Ct { get; set; } = new();


    /// <summary>
    /// Breakdown per image type.
    /// </summary>
    public Dictionary<string, int> PerType { get; set; }
}

public class MriStats
{
    /// <summary>
    /// Donors with the data / total.
    /// </summary>
    public int[] Number { get; set; }


    /// <summary>
    /// Breakdown per size.
    /// </summary>
    public Dictionary<string, int> PerSize { get; set; }
}

public class CtStats
{
    /// <summary>
    /// Donors with the data / total.
    /// </summary>
    public int[] Number { get; set; }
}
