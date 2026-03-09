using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Genes;

namespace Unite.Indices.Search.Engine;

public class GeneExpressionsIndexService(IElasticOptions options): IndexService<GeneExpressionIndex>(options)
{
    protected override string Collection => IndexNames.GeneExpressions;
}
