using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

public record GenesDataCriteria : CriteriaCollection
{
    public BoolCriteria HasSms { get; set; }
    public BoolCriteria HasCnvs { get; set; }
    public BoolCriteria HasCnvps { get; set; }
    public BoolCriteria HasSvs { get; set; }
}
