using Unite.Indices.Models;

namespace Unite.Indices.Entities.Projects;

public class DonorStatsIndex
{
    public int NumberOfDonors { get; set; }
    public Stat[] NumberOfDonorsByGender { get; set; }
    public Stat[] NumberOfDonorsByAge { get; set; }
    public Stat[] NumberOfDonorsByDiagnosis { get; set; }
    public double PercentOfDonorsVital { get; set; }
    public double PercentOfDonorsProgressing { get; set; }
}
