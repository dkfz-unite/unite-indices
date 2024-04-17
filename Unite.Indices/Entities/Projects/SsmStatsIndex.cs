namespace Unite.Indices.Entities.Projects;

public class SsmStatsIndex
{
    // Ssms // SSM types(SNV, INS, DEL, MNV)
    public int Total { get; set; } // Total number of donors having ssms 
    public Stat<int>[] ByType { get; set; }
    public Stat<int[]>[] ByGene { get; set; } // Gene = GeneCategory // integer list by high/moderate ... [highInt, modInt, ...] all of them following
    public Stat<int[]>[] SnvByGene { get; set; }
    public Stat<int[]>[] InsByGene { get; set; }
    public Stat<int[]>[] DelByGene { get; set; }
    public Stat<int[]>[] MnvByGene { get; set; }
}
