namespace Unite.Indices.Entities.Projects;

public class OrganoidStatsIndex
{
    public int Total { get; set; }
    public Stat<int>[] ByDrugDss { get; set; }
    public Stat<int>[] ByDrugSdss { get; set; }
    public Stat<int>[] ByInterventionType { get; set; }
}
