using Unite.Indices.Models;

namespace Unite.Indices.Entities.Projects;

public class ProjectIndex : Basic.Donors.ProjectIndex
{
    public int NumberOfDonors { get; set; }
    public Stat[] NumberOfDonorsByGender { get; set; }
    public Stat[] NumberOfDonorsByAge { get; set; }
    public Stat[] NumberOfDonorsByDiagnosis { get; set; }
    public double PercentOfDonorsVital { get; set; }
    public double PercentOfDonorsProgressing { get; set; }

    public int NumberOfMriImages { get; set; }

    public int NumberOfMaterials { get; set; }
    public int NumberOfLines { get; set; }
    public int NumberOfOrganoids { get; set; }
    public int NumberOfXenografts { get; set; }

    public int NumberOfSsms { get; set; }
    public Stat[] GenesAffectedBySsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public Stat[] GenesAffectedByCnvs { get; set; }
    public int NumberOfSvs { get; set; }
    public Stat[] GenesAffectedBySvs { get; set; }

    public int NumberOfTranscriptomics { get; set; }
}
