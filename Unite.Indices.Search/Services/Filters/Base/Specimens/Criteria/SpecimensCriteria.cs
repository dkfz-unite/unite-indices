using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;

public record SpecimensCriteria : CriteriaCollection
{
    public ValuesCriteria<int> Id { get; init; }
    public ValuesCriteria<string> ReferenceId { get; init; }
    public ValuesCriteria<string> SpecimenType { get; set; }

    public BoolCriteria HasExp { get; set; }
    public BoolCriteria HasExpSc { get; set; }
    public BoolCriteria HasSms { get; set; }
    public BoolCriteria HasCnvs { get; set; }
    public BoolCriteria HasSvs { get; set; }
    public BoolCriteria HasMeth { get; set; }
}
