using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Context;

public class CnvsIndexService(IElasticOptions options) : IndexService<CnvIndex>(options)
{
    protected override string Collection => IndexNames.Cnvs;
    protected override Expression<Func<CnvIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<CnvIndex>(m => m
                .AutoMap()
            )
        );
    }
}
