using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Context;

public class SvsIndexService(IElasticOptions options) : IndexService<SvIndex>(options)
{
    protected override string Collection => IndexNames.Svs;
    protected override Expression<Func<SvIndex, object>> Identifier => index => index.Id;
}
