using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Genes;
using Unite.Indices.Search.Engine.Enums;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Indices.Search.Services;

public class GenesSearchService : AggregatingSearchService, IGenesSearchService
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public override IIndexService<GeneIndex> GenesIndexService => _genesIndexService;
    public override IIndexService<VariantIndex> VariantsIndexService => _variantsIndexService;

    public GenesSearchService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }

    public GeneIndex Get(string key)
    {
        var query = new GetQuery<GeneIndex>(key)
            .AddExclusion(gene => gene.Specimens);

        var result = _genesIndexService.Get(query).Result;

        return result;
    }

    public IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var availableData = new Dictionary<int, DataIndex>();

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

    public SearchResult<GeneIndex> Search(SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var criteriaFilters = new GeneIndexFiltersCollection(criteria)
            .All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(gene => gene.NumberOfDonors)
            .AddExclusion(gene => gene.Specimens);

        var result = _genesIndexService.Search(query).Result;

        return result;
    }

    public SearchResult<DonorIndex> SearchDonors(int geneId, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.Gene = new GeneCriteria() { Id = [geneId] };

        // Should never be null or empty
        var ids = AggregateFromGenes(index => index.Specimens.First().Donor.Id, criteria);

        criteria.Donor = (criteria.Donor ?? new DonorCriteria()) with { Id = ids };

        var filters = new DonorIndexFiltersCollection(criteria)
            .All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFilters(filters)
            .AddOrdering(donor => donor.NumberOfGenes);

        var result = _donorsIndexService.Search(query).Result;

        return result;
    }

    public SearchResult<VariantIndex> SearchVariants(int geneId, VariantType type, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.Gene = new GeneCriteria { Id = [geneId] };

        var criteriaFilters = GetFiltersCollection(type, criteria)
            .All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(mutation => mutation.NumberOfDonors)
            .AddExclusion(mutation => mutation.Specimens);

        var result = _variantsIndexService.Search(query).Result;

        return result;
    }


    private static FiltersCollection<VariantIndex> GetFiltersCollection(VariantType type, SearchCriteria criteria)
    {
        return type switch
        {
            VariantType.SSM => new SsmIndexFiltersCollection(criteria),
            VariantType.CNV => new CnvIndexFiltersCollection(criteria),
            VariantType.SV => new SvIndexFiltersCollection(criteria),
            _ => new VariantFiltersCollection(criteria)
        };
    }
}
