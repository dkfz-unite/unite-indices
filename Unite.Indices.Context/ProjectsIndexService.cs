using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Projects;

namespace Unite.Indices.Context;

public class ProjectsIndexService(IElasticOptions options) : IndexService<ProjectIndex>(options)
{
    protected override string Collection => IndexNames.Projects;
    protected override Expression<Func<ProjectIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<ProjectIndex>(m => m
                .AutoMap()
            )
        );
    }
}
