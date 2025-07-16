using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;

public record GenesCriteria : CriteriaCollection
{
    public ValuesCriteria<int> Id { get; set; }

    public BoolCriteria HasSms { get; set; }
    public BoolCriteria HasCnvs { get; set; }
    public BoolCriteria HasSvs { get; set; }
}
