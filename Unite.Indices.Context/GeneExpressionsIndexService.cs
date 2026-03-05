using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Genes;

namespace Unite.Indices.Context;

public class GeneExpressionsIndexService(IElasticOptions options) : IndexService<GeneExpressionIndex>(options)
{
    protected override string Collection => IndexNames.GeneExpressions;

    protected override Expression<Func<GeneExpressionIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = _client.Indices.Exists(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<GeneExpressionIndex>(m => m
                .AutoMap()
            )
        );
    }
}
