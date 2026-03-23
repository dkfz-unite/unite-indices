using Nest;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.CnvProfiles;

namespace Unite.Indices.Search.Engine;

public class CnvProfileIndexService(IElasticOptions options) : IndexService<CnvProfileIndex>(options)
{
    protected override string Collection => IndexNames.CnvProfiles;
}