using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Context;

public class SmsIndexService(IElasticOptions options) : IndexService<SmIndex>(options)
{
    protected override string Collection => IndexNames.Sms;
    protected override Expression<Func<SmIndex, object>> Identifier => index => index.Id;
}
