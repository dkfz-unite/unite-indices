using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Donors;

namespace Unite.Indices.Context;

public class DonorsIndexService(IElasticOptions options) : IndexService<DonorIndex>(options)
{
    protected override string Collection => IndexNames.Donors;
    protected override Expression<Func<DonorIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<DonorIndex>(m => m
                .AutoMap()
                .Properties(p => p
                    .Nested<ImageIndex>(np => np
                        .Name(i => i.Images)
                        .AutoMap()
                    )
                    .Nested<SpecimenIndex>(np => np
                        .Name(i => i.Specimens)
                        .AutoMap()
                    )
                )
            )
        );
    }
}
