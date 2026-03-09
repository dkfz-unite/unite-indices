using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Genes;

namespace Unite.Indices.Context;

public class GenesIndexService(IElasticOptions options) : IndexService<GeneIndex>(options)
{
    protected override string Collection => IndexNames.Genes;
    protected override Expression<Func<GeneIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<GeneIndex>(m => m
                .AutoMap()
            )
        );
    }
}
