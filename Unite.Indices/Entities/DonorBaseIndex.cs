using System;

namespace Unite.Indices.Entities
{
    public class DonorBaseIndex
    {
        public string Id { get; set; }
        public string Diagnosis { get; set; }
        public DateTime? DiagnosisDate { get; set; }
        public string PrimarySite { get; set; }
        public string Origin { get; set; }
        public bool? MtaProtected { get; set; }

        public ClinicalDataIndex ClinicalData { get; set; }
        public TreatmentIndex[] Treatments { get; set; }
        public WorkPackageIndex[] WorkPackages { get; set; }
        public StudyIndex[] Studies { get; set; }
    }
}
