namespace Unite.Indices.Entities.Projects;

public class ProjectIndex : Basic.Donors.ProjectIndex
{
    public int NumberOfDonors { get; set; }
    public int NumberOfMriImages { get; set; }
    public int NumberOfMaterials { get; set; }
    public int NumberOfLines { get; set; }
    public int NumberOfOrganoids { get; set; }
    public int NumberOfXenografts { get; set; }

    // Still necessary with more detailed genes counters?
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }
    // --------------------------------------------------

    public int NumberOfTranscriptomics { get; set; }

    // Drafts
    public Dictionary<string, int> NumberOfDonorsByDiagnosis { get; set; } // Everything with empty values goes to "Other"
    public Dictionary<string, int> NumberOfDonorsByGender { get; set; }
    public Dictionary<int, int> NumberOfDonorsByAge { get; set; }
    public Dictionary<string, int> GenesAffectedBySsms { get; set; }
    public Dictionary<string, int> GenesAffectedByCnvs { get; set; }
    public Dictionary<string, int> GenesAffectedBySvs { get; set; }

}
