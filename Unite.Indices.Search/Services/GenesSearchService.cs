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


    public override async Task<GeneIndex> Get(string key)
    {
        var query = new GetQuery<GeneIndex>(key)
            .AddExclusion(gene => gene.Specimens);

        return await _genesIndexService.Get(query);
    }

    public override async Task<SearchResult<GeneIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var filters = new GeneFiltersCollection(criteria).All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(gene => gene.NumberOfDonors)
            .AddExclusion(gene => gene.Specimens.First().Donor)
            .AddExclusion(gene => gene.Specimens.First().Images.First().Mri)
            .AddExclusion(gene => gene.Specimens.First().Material)
            .AddExclusion(gene => gene.Specimens.First().Line)
            .AddExclusion(gene => gene.Specimens.First().Organoid)
            .AddExclusion(gene => gene.Specimens.First().Xenograft);

        return await _genesIndexService.Search(query);
    }


    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, GeneIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
