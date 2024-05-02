namespace Unite.Indices.Entities.Projects;

public class SsmStatsIndex
{
    public int Total { get; set; }
    public Stat<int>[] ByType { get; set; }
    public Stat<int[]>[] ByGene { get; set; }
    public Stat<int[]>[] SnvByGene { get; set; }
    public Stat<int[]>[] InsByGene { get; set; }
    public Stat<int[]>[] DelByGene { get; set; }
    public Stat<int[]>[] MnvByGene { get; set; }
}
