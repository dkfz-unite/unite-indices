using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Proteins;

namespace Unite.Indices.Search.Engine;

public class ProteinsIndexService(IElasticOptions options) : IndexService<ProteinIndex>(options)
{
    protected override string Collection => IndexNames.Proteins;
}
