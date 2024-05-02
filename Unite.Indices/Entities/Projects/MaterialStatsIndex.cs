namespace Unite.Indices.Entities.Projects;

public class MaterialStatsIndex
{
    public int Total { get; set; }
    public Stat<int>[] BySource { get; set; }
    public Stat<int>[] ByType { get; set; }
    public Stat<int>[] ByTumorType { get; set; }
}
