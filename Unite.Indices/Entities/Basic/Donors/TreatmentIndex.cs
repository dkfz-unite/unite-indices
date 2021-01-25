using System;

namespace Unite.Indices.Entities.Basic.Donors
{
    public class TreatmentIndex
    {
        public string Therapy { get; set; }

        public string Details { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Results { get; set; }
    }
}