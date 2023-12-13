using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Donors.DataIndex;


namespace Unite.Indices.Search.Services;

public class DonorsSearchService : AggregatingSearchService, IDonorsSearchService
{
    private readonly IIndexService<DonorIndex> _donorsIndexService;
    private readonly IIndexService<ImageIndex> _imagesIndexService;
    private readonly IIndexService<SpecimenIndex> _specimensIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public override IIndexService<GeneIndex> GenesIndexService => _genesIndexService;
    public override IIndexService<VariantIndex> VariantsIndexService => _variantsIndexService;
    

    public DonorsSearchService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _imagesIndexService = new ImagesIndexService(options);
        _specimensIndexService = new SpecimensIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }


    public DonorIndex Get(string key)
    {
        var query = new GetQuery<DonorIndex>(key)
            .AddExclusion(donor => donor.Specimens.First().Tissue)
            .AddExclusion(donor => donor.Specimens.First().Cell)
            .AddExclusion(donor => donor.Specimens.First().Organoid)
            .AddExclusion(donor => donor.Specimens.First().Xenograft);

        return _donorsIndexService.Get(query).Result;
    }

    public SearchResult<DonorIndex> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        int[] ids = null;

        if (criteria.HasGeneFilters)
            ids = AggregateFromGenes(index => index.Specimens.First().Donor.Id, criteria);
        else if (criteria.HasVariantFilters)
            ids = AggregateFromVariants(index => index.Specimens.First().Donor.Id, criteria);

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
            .AddExclusion(donor => donor.Specimens)
            .AddExclusion(donor => donor.Images);

        return _donorsIndexService.Search(query).Result;
    }

    public SearchResult<ImageIndex> SearchImages(SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria;

        var filters = new ImageFiltersCollection(criteria).All();

        var query = new SearchQuery<ImageIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters);

        return _imagesIndexService.Search(query).Result;
    }

    public SearchResult<SpecimenIndex> SearchSpecimens(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var filters = new SpecimenFiltersCollection(criteria).All();

        var query = new SearchQuery<SpecimenIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(specimen => specimen.NumberOfGenes);

        return _specimensIndexService.Search(query).Result;
    }

    public SearchResult<GeneIndex> SearchGenes(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var filters = new GeneFiltersCollection(criteria).All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(gene => gene.NumberOfDonors);

        return _genesIndexService.Search(query).Result;
    }

    public SearchResult<VariantIndex> SearchVariants(SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria;

        var filters = new VariantFiltersCollection(criteria).All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(mutation => mutation.NumberOfDonors);

        return _variantsIndexService.Search(query).Result;
    }

    public IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria with { From = 0, Size = 0 };

        var lookupResult = Search(criteria);

        var availableData = new Dictionary<int, DataIndex>();

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
