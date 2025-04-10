using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

public record DonorCriteria : DonorsCriteria
{
    public string[] Sex { get; set; }
    public Range<double?> Age { get; set; }
    public string[] Diagnosis { get; set; }
    public string[] PrimarySite { get; set; }
    public string[] Localization { get; set; }
    public bool? VitalStatus { get; set; }
    public Range<double?> VitalStatusChangeDay { get; set; }
    public bool? ProgressionStatus { get; set; }
    public Range<double?> ProgressionStatusChangeDay { get; set; }

    public string[] Therapy { get; set; }

    public bool? MtaProtected { get; set; }
    public string[] Project { get; set; }
    public string[] Study { get; set; }


    public override bool IsNotEmpty()
    {
        return base.IsNotEmpty()
            || Sex?.Length > 0
            || Age?.From != null
            || Age?.To != null
            || Diagnosis?.Length > 0
            || PrimarySite?.Length > 0
            || Localization?.Length > 0
            || VitalStatus != null
            || VitalStatusChangeDay?.From != null
            || VitalStatusChangeDay?.To != null
            || ProgressionStatus != null
            || ProgressionStatusChangeDay?.From != null
            || ProgressionStatusChangeDay?.To != null
            || Therapy?.Length > 0
            || MtaProtected != null
            || Project?.Length > 0
            || Study?.Length > 0;
    }
}
