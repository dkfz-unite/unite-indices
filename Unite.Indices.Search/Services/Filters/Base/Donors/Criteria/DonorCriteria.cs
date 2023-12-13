using Unite.Indices.Search.Services.Filters.Criteria.Models;

namespace Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;

public record DonorCriteria : DonorBaseCriteria
{
    public bool? HasSsms { get; set; }
    public bool? HasCnvs { get; set; }
    public bool? HasSvs { get; set; }
    public bool? HasGeneExp { get; set; }
}
