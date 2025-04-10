using Unite.Indices.Entities.Projects.Stats.Base;

namespace Unite.Indices.Entities.Projects.Stats;

public class DnaStats
{
    /// <summary>
    /// SM variants data.
    /// </summary>
    public SmStats Sm { get; set; } = new();

    /// <summary>
    /// CNV variants data.
    /// </summary>
    public CnvStats Cnv { get; set; } = new();

    /// <summary>
    /// SV variants data.
    /// </summary>
    public SvStats Sv { get; set; } = new();
}

public class SmStats
{
    /// <summary>
    /// Donors with SM variants.
    /// </summary>
    public int Number { get; set; }


    /// <summary>
    /// Breakdown of donors per analysis type.
    /// </summary>
    public Stat<string, int>[] PerAnalysis { get; set; }

    /// <summary>
    /// Breakdown per type.
    /// </summary>
    public Stat<string, int>[] PerType { get; set; }

    /// <summary>
    /// Breakdown per effect impact.
    /// </summary>
    public Stat<string, int>[] PerEffectImpact { get; set; }

    /// <summary>
    /// Breakdown per variant effect type.
    /// </summary>
    public Stat<string, int>[] PerEffectType { get; set; }

    /// <summary>
    /// Breakdown per ref base.
    /// </summary>
    public Stat<string, int>[] PerBaseRef { get; set; }

    /// <summary>
    /// Breakdown per alt base.
    /// </summary>
    public Stat<string, int>[] PerBaseAlt { get; set; }

    /// <summary>
    /// Breakdown per base change.
    /// </summary>
    public Stat<string, int>[] PerBaseChange { get; set; }
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
    public Stat<string, int>[] PerAnalysis { get; set; }

    /// <summary>
    /// Breakdown per type.
    /// </summary>
    public Stat<string, int>[] PerType { get; set; }
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
    public Stat<string, int>[] PerAnalysis { get; set; }

    /// <summary>
    /// Breakdown per type.
    /// </summary>
    public Stat<string, int>[] PerType { get; set; }
}
