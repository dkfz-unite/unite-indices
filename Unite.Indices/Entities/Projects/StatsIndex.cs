using Unite.Indices.Entities.Projects.Stats;

namespace Unite.Indices.Entities.Projects;

public class StatsIndex
{
    /// <summary>
    /// Donors data.
    /// </summary>
    public DonorsStats Donors { get; set; } = new();

    /// <summary>
    /// Images data.
    /// </summary>
    public ImagesStats Images { get; set; } = new();

    /// <summary>
    /// Specimens data.
    /// </summary>
    public SpecimensStats Specimens { get; set; } = new();

    /// <summary>
    /// DNA data.
    /// </summary>
    public DnaStats Dna { get; set; } = new();

    /// <summary>
    /// Methylation data.
    /// </summary>
    public MethStats Meth { get; set; } = new();

    /// <summary>
    /// Bulk RNA data.
    /// </summary>
    public RnaStats Rna { get; set; } = new();

    /// <summary>
    /// Single-cell RNA data.
    /// </summary>
    public RnascStats Rnasc { get; set; } = new();
}
