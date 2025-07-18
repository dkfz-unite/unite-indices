using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

public record DonorsCriteria : CriteriaCollection
{
    public ValuesCriteria<int> Id { get; set; }
    public ValuesCriteria<string> ReferenceId { get; set; }

    public BoolCriteria HasExp { get; set; }
    public BoolCriteria HasExpSc { get; set; }
    public BoolCriteria HasSms { get; set; }
    public BoolCriteria HasCnvs { get; set; }
    public BoolCriteria HasSvs { get; set; }
    public BoolCriteria HasMeth { get; set; }
}
