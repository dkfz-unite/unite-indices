using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Projects;

namespace Unite.Indices.Context;

public class ProjectsIndexService(IElasticOptions options) : IndexService<ProjectIndex>(options)
{
    protected override string Collection => IndexNames.Projects;
    protected override Expression<Func<ProjectIndex, object>> Identifier => index => index.Id;
}
