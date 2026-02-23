using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Proteins;

namespace Unite.Indices.Context;

public class ProteinsIndexService(IElasticOptions options) : IndexService<ProteinIndex>(options)
{
    protected override string Collection => IndexNames.Proteins;
    protected override Expression<Func<ProteinIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<ProteinIndex>(m => m
                .AutoMap()
                .Properties(p => p
                    .Nested<SpecimenIndex>(np => np
                        .Name(i => i.Specimens)
                        .AutoMap()
                    )
                )
            )
        );
    }
}
