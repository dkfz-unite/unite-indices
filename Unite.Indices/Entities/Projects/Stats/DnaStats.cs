namespace Unite.Indices.Entities.Projects.Stats;

public class DnaStats
{
    /// <summary>
    /// SSM variants data.
    /// </summary>
    public SsmStats Ssm { get; set; } = new();

    /// <summary>
    /// CNV variants data.
    /// </summary>
    public CnvStats Cnv { get; set; } = new();

    /// <summary>
    /// SV variants data.
    /// </summary>
    public SvStats Sv { get; set; } = new();
}

public class SsmStats
{
    /// <summary>
    /// Donors with SSM variants.
    /// </summary>
    public int Number { get; set; }


    /// <summary>
    /// Breakdown of donors per analysis type.
    /// </summary>
    public Dictionary<string, int> PerAnalysis { get; set; }

    /// <summary>
    /// Breakdown per type.
    /// </summary>
    public Dictionary<string, int> PerType { get; set; }

    /// <summary>
    /// Breakdown per effect impact.
    /// </summary>
    public Dictionary<string, int> PerEffectImpact { get; set; }

    /// <summary>
    /// Breakdown per variant effect type.
    /// </summary>
    public Dictionary<string, int> PerEffectType { get; set; }

    /// <summary>
    /// Breakdown per ref base.
    /// </summary>
    public Dictionary<string, int> PerBaseRef { get; set; }

    /// <summary>
    /// Breakdown per alt base.
    /// </summary>
    public Dictionary<string, int> PerBaseAlt { get; set; }
}

public class CnvStats
{
    /// <summary>
    /// Donors with CNV variants.
    /// </summary>
    public int Number { get; set; }


    /// <summary>
    /// Breakdown of donors per analysis type.
    /// </summary>
    public Dictionary<string, int> PerAnalysis { get; set; }

    /// <summary>
    /// Breakdown per type.
    /// </summary>
    public Dictionary<string, int> PerType { get; set; }
}

public class SvStats
{
    /// <summary>
    /// Donors with SV variants.
    /// </summary>
    public int Number { get; set; }


    /// <summary>
    /// Breakdown of donors per analysis type.
    /// </summary>
    public Dictionary<string, int> PerAnalysis { get; set; }

    /// <summary>
    /// Breakdown per type.
    /// </summary>
    public Dictionary<string, int> PerType { get; set; }
}
