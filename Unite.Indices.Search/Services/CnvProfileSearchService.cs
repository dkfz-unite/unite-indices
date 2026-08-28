using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities;
using Unite.Indices.Entities.CnvProfiles;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Base.Variants;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class CnvProfileSearchService: SearchService<CnvProfileIndex>
{
    public CnvProfileSearchService(IElasticOptions options) : base(options)
    {
    }
    
    protected override SearchCriteria BuildSearchCriteria(int id)
    {
        return new SearchCriteria
        {
            CnvProfile = new CnvProfileCriteria()
        };
    }

    protected override IIndexService<CnvProfileIndex> GetIndexService()
    {
        return _cnvProfileIndexService;
    }

    public override async Task<SearchResult<CnvProfileIndex>> Search(PersonalSearchCriteria personalSearchCriteria)
    {
        var searchCriteria = personalSearchCriteria?.SearchCriteria;
        var criteria = searchCriteria ?? new SearchCriteria();

        var filters = new CnvProfileFilters<CnvProfileIndex>(criteria.CnvProfile, cnvProfile => cnvProfile).All();

        var query = new SearchQuery<CnvProfileIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters);
        
        return await _cnvProfileIndexService.Search(query);
    }

    protected override void AddToStats(ref Dictionary<object, DataIndex> stats, CnvProfileIndex index)
    {
        //TODO: add data to stats
        //stats.Add(index.Id, index.);
    }
}