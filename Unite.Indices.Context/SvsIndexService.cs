using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Context;

public class SvsIndexService(IElasticOptions options) : IndexService<SvIndex>(options)
{
    protected override string Collection => IndexNames.Svs;
    protected override Expression<Func<SvIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<SvIndex>(m => m
                .AutoMap()
            )
        );
    }
}
