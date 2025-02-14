using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Search.Engine;

public class SvsIndexService(IElasticOptions options) : IndexService<SvIndex>(options)
{
    protected override string Collection => IndexNames.Svs;
}
