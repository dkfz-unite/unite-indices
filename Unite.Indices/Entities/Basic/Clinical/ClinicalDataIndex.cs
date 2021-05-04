using System;

namespace Unite.Indices.Entities.Basic.Clinical
{
    public class ClinicalDataIndex
    {
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string Diagnosis { get; set; }
        public DateTime? DiagnosisDate { get; set; }
        public string PrimarySite { get; set; }
        public string Localization { get; set; }
        public bool? VitalStatus { get; set; }
        public DateTime? VitalStatusChangeDate { get; set; }
        public int? KpsBaseline { get; set; }
        public bool? SteroidsBaseline { get; set; }
    }
}