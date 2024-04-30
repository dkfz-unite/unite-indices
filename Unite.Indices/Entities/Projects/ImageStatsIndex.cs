namespace Unite.Indices.Entities.Projects;

public class ImageStatsIndex
{
    public int Total { get; set; }
    public Stat<int>[] ByVolume { get; set; }
}
