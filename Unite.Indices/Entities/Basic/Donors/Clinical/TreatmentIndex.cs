namespace Unite.Indices.Entities.Basic.Donors.Clinical
{
    public class TreatmentIndex
    {
        public string Therapy { get; set; }
        public string Details { get; set; }
        public int? StartDay { get; set; }
        public int? DurationDays { get; set; }
        public string Results { get; set; }
    }
}