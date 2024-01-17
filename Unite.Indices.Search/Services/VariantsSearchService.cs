using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class VariantsSearchService : SearchService<VariantIndex>
{
    public VariantsSearchService(IElasticOptions options) : base(options)
    {
    }

    public override async Task<VariantIndex> Get(string key)
    {
        var query = new GetQuery<VariantIndex>(key)
            .AddExclusion(variant => variant.Specimens);

        return await _variantsIndexService.Get(query);
    }

    public override async Task<SearchResult<VariantIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var filters = new VariantFiltersCollection(criteria).All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(variant => variant.NumberOfDonors)
            .AddExclusion(variant => variant.Specimens.First().Donor)
            .AddExclusion(variant => variant.Specimens.First().Images.First().Mri)
            .AddExclusion(variant => variant.Specimens.First().Material)
            .AddExclusion(variant => variant.Specimens.First().Line)
            .AddExclusion(variant => variant.Specimens.First().Organoid)
            .AddExclusion(variant => variant.Specimens.First().Xenograft);

        return await _variantsIndexService.Search(query);
    }


    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, VariantIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
