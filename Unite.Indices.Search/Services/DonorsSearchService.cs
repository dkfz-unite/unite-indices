using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class DonorsSearchService : SearchService<DonorIndex>
{
    public DonorsSearchService(IElasticOptions options) : base(options)
    {
    }


    public override async Task<DonorIndex> Get(string key)
    {
        var query = new GetQuery<DonorIndex>(key);

        return await _donorsIndexService.Get(query);
    }

    public override async Task<SearchResult<DonorIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        int[] ids = null;

        if (criteria.HasGeneFilters && !criteria.HasVariantFilters)
            ids = await AggregateFromGenes(index => index.Specimens.First().Donor.Id, criteria) ?? [];
        else if ((criteria.HasGeneFilters && criteria.HasVariantFilters) || criteria.HasVariantFilters)
            ids = await AggregateFromVariants(index => index.Specimens.First().Donor.Id, criteria) ?? [];

        if (ids != null)
        {
            if (ids.Length > 0)
                criteria.Donor = (criteria.Donor ?? new DonorCriteria()) with { Id = ids };
            else
                return new SearchResult<DonorIndex>();
        }

        var filters = new DonorFiltersCollection(criteria).All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(donor => donor.NumberOfGenes)
            .AddExclusion(donor => donor.Images)
            .AddExclusion(donor => donor.Specimens.First().Material)
            .AddExclusion(donor => donor.Specimens.First().Line)
            .AddExclusion(donor => donor.Specimens.First().Organoid)
            .AddExclusion(donor => donor.Specimens.First().Xenograft);

        return await _donorsIndexService.Search(query);
    }

    
    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, DonorIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
