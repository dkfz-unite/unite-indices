using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Search.Engine;

public class CnvsIndexService(IElasticOptions options) : IndexService<CnvIndex>(options)
{
    protected override string Collection => IndexNames.Cnvs;
}
