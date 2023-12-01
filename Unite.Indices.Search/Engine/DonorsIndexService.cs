using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Donors;

namespace Unite.Indices.Search.Engine;

public class DonorsIndexService(IElasticOptions options) : IndexService<DonorIndex>(options)
{
    protected override string Collection => IndexNames.Donors;
}
