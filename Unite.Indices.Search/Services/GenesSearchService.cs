using Unite.Indices.Entities.Genes;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class GenesSearchService : SearchService<GeneIndex>
{
    public GenesSearchService(IElasticOptions options) : base(options)
    {
    }


    public override GeneIndex Get(string key)
    {
        var query = new GetQuery<GeneIndex>(key)
            .AddExclusion(gene => gene.Specimens);

        return _genesIndexService.Get(query).Result;
    }

    public override SearchResult<GeneIndex> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var filters = new GeneFiltersCollection(criteria).All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(gene => gene.NumberOfDonors)
            .AddExclusion(gene => gene.Specimens);

        return _genesIndexService.Search(query).Result;
    }


    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, GeneIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
