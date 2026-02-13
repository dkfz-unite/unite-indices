using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base;

public record DataCriteria : CriteriaCollection
{
    public BoolCriteria HasExp { get; set; }
    public BoolCriteria HasExpSc { get; set; }
    public BoolCriteria HasSms { get; set; }
    public BoolCriteria HasCnvs { get; set; }
    public BoolCriteria HasSvs { get; set; }
    public BoolCriteria HasMeth { get; set; }
    public BoolCriteria HasProt { get; set; }
}
