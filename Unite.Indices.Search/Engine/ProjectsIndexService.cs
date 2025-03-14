using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Projects;

namespace Unite.Indices.Search.Engine;

public class ProjectsIndexService(IElasticOptions options) : IndexService<ProjectIndex>(options)
{
    protected override string Collection => IndexNames.Projects;
}
