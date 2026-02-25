using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Context.Constants;
using Unite.Indices.Entities.CnvProfiles;

namespace Unite.Indices.Context;

public class CnvProfilesIndexService(IElasticOptions options): IndexService<CnvProfileIndex>(options)
{
    protected override string Collection => IndexNames.CnvProfiles;
    protected override Expression<Func<CnvProfileIndex, object>> Identifier => index => index.Id;
    
    public override async Task CreateIndex()
    {
        var existsResponse = await _client.Indices.ExistsAsync(Collection);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(Collection, c => c
            .Map<CnvProfileIndex>(m => m.AutoMap())
        );
        
        //TODO: Validate createResponse and report errors
    }
}