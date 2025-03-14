namespace Unite.Indices.Entities.Projects;

public class ProjectIndex : Basic.Projects.ProjectIndex
{
    /// <summary>
    /// Statistics.
    /// </summary>
    public StatsIndex Stats { get; set; }

    /// <summary>
    /// Available data.
    /// </summary>
    public DataIndex Data { get; set; }


    public DonorIndex[] Donors { get; set; }
}
