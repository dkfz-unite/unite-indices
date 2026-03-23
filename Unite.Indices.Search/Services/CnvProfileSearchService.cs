using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities;
using Unite.Indices.Entities.CnvProfiles;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Base.Variants;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class CnvProfileSearchService: SearchService<CnvProfileIndex>
{
    public CnvProfileSearchService(IElasticOptions options) : base(options)
    {
    }

    public override Task<CnvProfileIndex> Get(string key)
    {
        throw new NotImplementedException();
    }

    public override async Task<SearchResult<CnvProfileIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var filters = new CnvProfileFilters(criteria.CnvProfileCriteria).All();
        
        var query = new SearchQuery<CnvProfileIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(cnvProfile => cnvProfile.Chromosome);
        
        return await _cnvProfileIndexService.Search(query);
    }

    protected override void AddToStats(ref Dictionary<object, DataIndex> stats, CnvProfileIndex index)
    {
        //TODO: add data to stats
        //stats.Add(index.Id, index.);
    }
}