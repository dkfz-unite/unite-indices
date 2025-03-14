using Unite.Indices.Entities.Basic.Projects;

namespace Unite.Indices.Entities.Basic.Donors;

public class DonorIndex : DonorNavIndex
{      
    public bool? MtaProtected { get; set; }

    public ClinicalDataIndex ClinicalData { get; set; }
    public TreatmentIndex[] Treatments { get; set; }
    public ProjectNavIndex[] Projects { get; set; }
    public StudyNavIndex[] Studies { get; set; }
}
