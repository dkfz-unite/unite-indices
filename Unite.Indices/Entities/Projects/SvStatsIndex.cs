namespace Unite.Indices.Entities.Projects;

public class SvStatsIndex
{
    public int Total { get; set; }
    public Stat<int[]>[] DupByRange { get; set; }
    public Stat<int[]>[] TDupByRange { get; set; }
    public Stat<int[]>[] InsByRange { get; set; }
    public Stat<int[]>[] DelByRange { get; set; }
    public Stat<int[]>[] InvByRange { get; set; }
    public Stat<int[]>[] ItxByGene { get; set; }
    public Stat<int[]>[] CtxByGene { get; set; }
}
