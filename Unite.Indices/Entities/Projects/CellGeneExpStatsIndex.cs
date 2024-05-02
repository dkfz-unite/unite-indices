namespace Unite.Indices.Entities.Projects;

public class CellGeneExpStatsIndex
{
    public int Total { get; set; }
    public Stat<int>[] ByCellsNumber { get; set; }
    public Stat<int>[] ByGenesModel { get; set; }
}
