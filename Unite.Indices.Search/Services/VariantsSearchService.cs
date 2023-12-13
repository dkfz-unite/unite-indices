using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Indices.Search.Services;

public class VariantsSearchService : AggregatingSearchService, IVariantsSearchService
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public override IIndexService<GeneIndex> GenesIndexService => _genesIndexService;
    public override IIndexService<VariantIndex> VariantsIndexService => _variantsIndexService;


    public VariantsSearchService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }

    
    public VariantIndex Get(string key)
    {
        var query = new GetQuery<VariantIndex>(key)
            .AddExclusion(variant => variant.Specimens);

        return _variantsIndexService.Get(query).Result;
    }

    public SearchResult<VariantIndex> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var filters = new VariantFiltersCollection(criteria).All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(variant => variant.NumberOfDonors)
            .AddExclusion(variant => variant.Specimens);

        return _variantsIndexService.Search(query).Result;
    }

    public SearchResult<DonorIndex> SearchDonors(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        // Should never be null or empty
        var ids = AggregateFromVariants(index => index.Specimens.First().Donor.Id, criteria);

        criteria.Donor = (criteria.Donor ?? new DonorCriteria()) with { Id = ids };

        var filters = new DonorFiltersCollection(criteria).All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(donor => donor.NumberOfGenes)
            .AddExclusion(donor => donor.Specimens);

        return _donorsIndexService.Search(query).Result;
    }

    public IDictionary<string, DataIndex> Stats(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var availableData = new Dictionary<string, DataIndex>();

        criteria = criteria with { From = 0, Size = 0 };

        var lookupResult = Search(criteria);

        for (var from = 0; from < lookupResult.Total; from += 499)
        {
            criteria = criteria with { From = from, Size = 499 };

            var searchResult = Search(criteria);

            foreach (var index in searchResult.Rows)
            {
                availableData.Add(index.Id, index.Data);
            }
        }

        return availableData;
    }
}
