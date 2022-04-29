namespace Unite.Indices.Entities.Basic.Donors.Clinical
{
    public class ClinicalDataIndex
    {
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string Diagnosis { get; set; }
        public string PrimarySite { get; set; }
        public string Localization { get; set; }
        public bool? VitalStatus { get; set; }
        public int? VitalStatusChangeDay { get; set; }
        public bool? ProgressionStatus { get; set; }
        public int? ProgressionStatusChangeDay { get; set; }
        public int? KpsBaseline { get; set; }
        public bool? SteroidsBaseline { get; set; }
    }
}