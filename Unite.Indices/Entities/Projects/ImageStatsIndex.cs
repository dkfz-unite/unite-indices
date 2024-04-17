namespace Unite.Indices.Entities.Projects;

public class ImageStatsIndex
{
    public int Total { get; set; }
    public Stat<int>[] ByVolume { get; set; } // volume of tumor ... 0-10, 10-20 ... maybe bigger ranges ... open question
}
