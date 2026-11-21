using Unite.Indices.Entities.Basic.Projects;

namespace Unite.Indices.Entities.Basic.Donors;

public class DonorIndex : DonorNavIndex
{      
    [Searchable]
    public bool? MtaProtected { get; set; }

    [Searchable]
    public ClinicalDataIndex ClinicalData { get; set; }
    [Searchable]
    public TreatmentIndex[] Treatments { get; set; }
    public ProjectNavIndex[] Projects { get; set; }
    [Searchable]
    public StudyNavIndex[] Studies { get; set; }
}
