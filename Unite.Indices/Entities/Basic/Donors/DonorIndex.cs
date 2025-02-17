namespace Unite.Indices.Entities.Basic.Donors;

public class DonorIndex : DonorNavIndex
{      
    public bool? MtaProtected { get; set; }

    public ClinicalDataIndex ClinicalData { get; set; }
    public TreatmentIndex[] Treatments { get; set; }
    public ProjectIndex[] Projects { get; set; }
    public StudyIndex[] Studies { get; set; }
}
