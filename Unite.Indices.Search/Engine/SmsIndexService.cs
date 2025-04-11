using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Search.Engine;

public class SmsIndexService(IElasticOptions options) : IndexService<SmIndex>(options)
{
    protected override string Collection => IndexNames.Sms;
}
