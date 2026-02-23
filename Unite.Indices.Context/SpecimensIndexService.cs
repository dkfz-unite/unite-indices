using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Specimens;

namespace Unite.Indices.Context;

public class SpecimensIndexService(IElasticOptions options) : IndexService<SpecimenIndex>(options)
{
    protected override string Collection => IndexNames.Specimens;
    protected override Expression<Func<SpecimenIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<SpecimenIndex>(m => m
                .AutoMap()
                .Properties(p => p
                    .Nested<ImageIndex>(np => np
                        .Name(i => i.Images)
                        .AutoMap()
                    )
                )
            )
        );
    }
}
