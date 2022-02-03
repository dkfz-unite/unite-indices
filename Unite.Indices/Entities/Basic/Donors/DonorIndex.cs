using Unite.Indices.Entities.Basic.Donors.Clinical;

namespace Unite.Indices.Entities.Basic.Donors
{
    public class DonorIndex
    {
        public int Id { get; set; }
        public string ReferenceId { get; set; }
        public bool? MtaProtected { get; set; }

        public ClinicalDataIndex ClinicalData { get; set; }
        public TreatmentIndex[] Treatments { get; set; }
        public WorkPackageIndex[] WorkPackages { get; set; }
        public StudyIndex[] Studies { get; set; }
    }
}
