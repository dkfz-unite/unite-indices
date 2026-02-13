using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Proteins;

namespace Unite.Indices.Context;

public class ProteinsIndexService(IElasticOptions options) : IndexService<ProteinIndex>(options)
{
    protected override string Collection => IndexNames.Proteins;
    protected override Expression<Func<ProteinIndex, object>> Identifier => index => index.Id;
}
