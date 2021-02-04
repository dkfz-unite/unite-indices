using System;
using Unite.Indices.Entities.Basic.Epigenetics;

namespace Unite.Indices.Entities.Basic.Donors
{
    public class DonorIndex
    {
        public string Id { get; set; }
        public string Diagnosis { get; set; }
        public DateTime? DiagnosisDate { get; set; }
        public string PrimarySite { get; set; }
        public string Origin { get; set; }
        public bool? MtaProtected { get; set; }

        public ClinicalDataIndex ClinicalData { get; set; }
        public EpigeneticsDataIndex EpigeneticsData { get; set; }
        public TreatmentIndex[] Treatments { get; set; }
        public WorkPackageIndex[] WorkPackages { get; set; }
        public StudyIndex[] Studies { get; set; }
    }
}
