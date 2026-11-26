using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Proteins.Criteria;

public record class ProteinsDataCriteria : CriteriaCollection
{
    public BoolCriteria HasSms { get; set; }
    public BoolCriteria HasCnvs { get; set; }
    public BoolCriteria HasCnvps { get; set; }
    public BoolCriteria HasSvs { get; set; }
}
