using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Donors;

namespace Unite.Indices.Context;

public class DonorsIndexService(IElasticOptions options) : IndexService<DonorIndex>(options)
{
    protected override string Collection => IndexNames.Donors;
    protected override Expression<Func<DonorIndex, object>> Identifier => index => index.Id;
}
