using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class SsmsSearchService : SearchService<SsmIndex>
{
    public SsmsSearchService(IElasticOptions options) : base(options)
    {
    }

    public override async Task<SsmIndex> Get(string key)
    {
        var query = new GetQuery<SsmIndex>(key)
            .AddExclusion(variant => variant.Specimens);

        return await _ssmsIndexService.Get(query);
    }

    public override async Task<SearchResult<SsmIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        if (criteria.HasDonorFilters)
        {
            var ids = await AggregateFromDonors(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, ids.Select(int.Parse).ToArray());

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<SsmIndex>();
        }

        if (criteria.HasImageFilters)
        {
            var ids = await AggregateFromImages(index => index.Specimens.First().Id, criteria with { Specimen = null });

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, ids.Select(int.Parse).ToArray());

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<SsmIndex>();
        }

        if (criteria.HasSpecimenFilters)
        {
            var ids = await AggregateFromSpecimens(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, ids.Select(int.Parse).ToArray());

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<SsmIndex>();
        }

        var filters = new SsmFiltersCollection(criteria).All();

        var query = new SearchQuery<SsmIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(variant => variant.Stats.Donors);

        return await _ssmsIndexService.Search(query);
    }

    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, SsmIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
