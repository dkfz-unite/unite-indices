namespace Unite.Indices.Entities.Projects;

public class XenograftStatsIndex
{
    public int TotalXenografts { get; set; }
    public Stat<int>[] ByDrugDss { get; set; }
    public Stat<int>[] ByDrugSdss { get; set; }
}
