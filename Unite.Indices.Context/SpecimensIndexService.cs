using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Indices.Context;

public class SpecimensIndexService(IElasticOptions options) : IndexService<SpecimenIndex>(options)
{
    protected override string Collection => IndexNames.Specimens;
    protected override Expression<Func<SpecimenIndex, object>> Identifier => index => index.Id;
}
