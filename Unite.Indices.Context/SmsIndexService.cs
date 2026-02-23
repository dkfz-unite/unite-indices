using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.Variants;

namespace Unite.Indices.Context;

public class SmsIndexService(IElasticOptions options) : IndexService<SmIndex>(options)
{
    protected override string Collection => IndexNames.Sms;
    protected override Expression<Func<SmIndex, object>> Identifier => index => index.Id;

    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<SmIndex>(m => m
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
