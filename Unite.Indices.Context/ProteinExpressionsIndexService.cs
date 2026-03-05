using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Proteins;

namespace Unite.Indices.Context;

public class ProteinExpressionsIndexService(IElasticOptions options) : IndexService<ProteinExpressionIndex>(options)
{
    protected override string Collection => IndexNames.ProteinExpressions;
    protected override Expression<Func<ProteinExpressionIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<ProteinExpressionIndex>(m => m
                .AutoMap()
            )
        );
    }
}
