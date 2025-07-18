using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

public record DonorCriteria : DonorsCriteria
{
    public ValuesCriteria<string> Sex { get; set; }
    public RangeCriteria<double?> Age { get; set; }
    public ValuesCriteria<string> Diagnosis { get; set; }
    public ValuesCriteria<string> PrimarySite { get; set; }
    public ValuesCriteria<string> Localization { get; set; }
    public BoolCriteria VitalStatus { get; set; }
    public RangeCriteria<double?> VitalStatusChangeDay { get; set; }
    public BoolCriteria ProgressionStatus { get; set; }
    public RangeCriteria<double?> ProgressionStatusChangeDay { get; set; }

    public ValuesCriteria<string> Therapy { get; set; }

    public BoolCriteria MtaProtected { get; set; }
    public ValuesCriteria<string> Project { get; set; }
    public ValuesCriteria<string> Study { get; set; }
}
