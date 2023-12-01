using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Context;

public class VariantsIndexService(IElasticOptions options) : IndexService<VariantIndex>(options)
{
    protected override string Collection => IndexNames.Variants;
    protected override Expression<Func<VariantIndex, object>> Identifier => index => index.Id;
}
