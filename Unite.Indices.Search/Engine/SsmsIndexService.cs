using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Search.Engine;

public class SsmsIndexService(IElasticOptions options) : IndexService<SsmIndex>(options)
{
    protected override string Collection => IndexNames.Ssms;
}
