using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Images;

namespace Unite.Indices.Search.Engine;

public class ImagesIndexService(IElasticOptions options) : IndexService<ImageIndex>(options)
{
    protected override string Collection => IndexNames.Images;
}
