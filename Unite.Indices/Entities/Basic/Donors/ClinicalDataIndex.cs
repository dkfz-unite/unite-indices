using System;

namespace Unite.Indices.Entities.Basic.Donors
{
    public class ClinicalDataIndex
    {
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string AgeCategory { get; set; }
        public string Localization { get; set; }
        public string VitalStatus { get; set; }
        public DateTime? VitalStatusChangeDate { get; set; }
        public int? SurvivalDays { get; set; }
        public DateTime? ProgressionDate { get; set; }
        public int? ProgressionFreeDays { get; set; }
        public DateTime? RelapseDate { get; set; }
        public int? RelapseFreeDays { get; set; }
        public int? KpsBaseline { get; set; }
        public bool? SteroidsBaseline { get; set; }
    }
}