namespace Unite.Indices.Entities.Projects.Stats;

public class SpecimensStats
{
    /// <summary>
    /// Materials data.
    /// </summary>
    public MaterialStats Material { get; set; } = new();

    /// <summary>
    /// Cell lines data.
    /// </summary>
    public LineStats Line { get; set; } = new();

    /// <summary>
    /// Organoids data.
    /// </summary>
    public OrganoidStats Organoid { get; set; } = new();

    /// <summary>
    /// Xenografts data.
    /// </summary>
    public XenograftStats Xenograft { get; set; } = new();


    /// <summary>
    /// Breakdown per specimen type.
    /// </summary>
    public Dictionary<string, int> PerType { get; set; }
}

public class MaterialStats
{
    /// <summary>
    /// Donors with the data / total.
    /// </summary>
    public int[] Number { get; set; }
}

public class LineStats
{
    /// <summary>
    /// Donors with the data / total.
    /// </summary>
    public int[] Number { get; set; }
}

public class OrganoidStats
{
    /// <summary>
    /// Donors with the data / total.
    /// </summary>
    public int[] Number { get; set; }
}

public class XenograftStats
{
    /// <summary>
    /// Donors with the data / total.
    /// </summary>
    public int[] Number { get; set; }
}
