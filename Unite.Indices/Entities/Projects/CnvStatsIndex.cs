namespace Unite.Indices.Entities.Projects;

public class CnvStatsIndex
{
    public int Total { get; set; }
    public Stat<int[]>[] GainByRange { get; set; }
    public Stat<int[]>[] GainLohByRange { get; set; }
    public Stat<int[]>[] NeutralLohByRange { get; set; }
    public Stat<int[]>[] LossByRange { get; set; }
    public Stat<int[]>[] LossDelByRange { get; set; }
    public Stat<int[]>[] LossLohByRange { get; set; }
}
