namespace Unite.Indices.Entities.Projects;

public class ProjectIndex : Basic.Donors.ProjectIndex
{
    public int NumberOfDonors { get; set; }
    public int NumberOfMriImages { get; set; }
    public int NumberOfMaterials { get; set; }
    public int NumberOfLines { get; set; }
    public int NumberOfOrganoids { get; set; }
    public int NumberOfXenografts { get; set; }
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }
    public int NumberOfTranscriptomics { get; set; }
}
