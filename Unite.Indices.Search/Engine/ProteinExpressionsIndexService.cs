using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Proteins;

namespace Unite.Indices.Search.Engine;

public class ProteinExpressionsIndexService(IElasticOptions options): IndexService<ProteinExpressionIndex>(options)
{
    protected override string Collection => IndexNames.ProteinExpressions;
}
