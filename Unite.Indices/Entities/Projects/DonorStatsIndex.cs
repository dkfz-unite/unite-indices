namespace Unite.Indices.Entities.Projects;

public class DonorStatsIndex
{
    public int Total { get; set; }
    public Stat<int>[] ByGender { get; set; }
    public Stat<int>[] ByAge { get; set; }
    public Stat<int>[] ByDiagnosis { get; set; }
    public Stat<int>[] ByVitalStatus { get; set; }
    public Stat<int>[] ByProgressionStatus { get; set; }
}
