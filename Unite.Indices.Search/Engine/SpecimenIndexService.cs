using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Indices.Search.Engine;

public class SpecimensIndexService(IElasticOptions options) : IndexService<SpecimenIndex>(options)
{
    protected override string Collection => IndexNames.Specimens;
}
