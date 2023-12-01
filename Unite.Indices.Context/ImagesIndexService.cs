using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Images;

namespace Unite.Indices.Context;

public class ImagesIndexService(IElasticOptions options) : IndexService<ImageIndex>(options)
{
    protected override string Collection => IndexNames.Images;
    protected override Expression<Func<ImageIndex, object>> Identifier => index => index.Id;
}
