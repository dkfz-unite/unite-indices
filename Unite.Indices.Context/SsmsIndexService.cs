using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Context;

public class SsmsIndexService(IElasticOptions options) : IndexService<SsmIndex>(options)
{
    protected override string Collection => IndexNames.Ssms;
    protected override Expression<Func<SsmIndex, object>> Identifier => index => index.Id;
}
