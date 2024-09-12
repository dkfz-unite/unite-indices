using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

public abstract record DonorBaseCriteria
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }

    public string[] Gender { get; set; }
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


    public virtual bool IsNotEmpty()
    {
        return Id?.Any() == true
            || ReferenceId?.Any() == true
            || Gender?.Any() == true
            || Age?.From != null
            || Age?.To != null
            || Diagnosis?.Any() == true
            || PrimarySite?.Any() == true
            || Localization?.Any() == true
            || VitalStatus != null
            || VitalStatusChangeDay?.From != null
            || VitalStatusChangeDay?.To != null
            || ProgressionStatus != null
            || ProgressionStatusChangeDay?.From != null
            || ProgressionStatusChangeDay?.To != null
            || Therapy?.Any() == true
            || MtaProtected != null
            || Project?.Any() == true
            || Study?.Any() == true;
    } 
}
