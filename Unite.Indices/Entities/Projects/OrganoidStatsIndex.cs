namespace Unite.Indices.Entities.Projects;

public class OrganoidStatsIndex
{
    public int TotalOrganoids { get; set; }
    public Stat<int>[] ByDrugDss { get; set; }
    public Stat<int>[] ByDrugSdss { get; set; }
}
