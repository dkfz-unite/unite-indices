using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Enums;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base;

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

        var result = _donorsIndexService.Get(query).Result;

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

    public SearchResult<DonorIndex> Search(SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var ids = AggregateFromVariants(index => index.Specimens.First().Donor.Id, criteria)
               ?? AggregateFromGenes(index => index.Specimens.First().Donor.Id, criteria)
               ?? null;
        
        if (ids?.Length == 0) 
        {
            return new SearchResult<DonorIndex>();
        }
        else
        {
            if (ids != null)
            {
                criteria.Donor = (criteria.Donor ?? new DonorCriteria()) with { Id = ids };
            }

            var filters = new DonorIndexFiltersCollection(criteria)
                .All();

            var query = new SearchQuery<DonorIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(filters)
                .AddOrdering(donor => donor.NumberOfGenes)
                .AddExclusion(donor => donor.Specimens)
                .AddExclusion(donor => donor.Images);

            var result = _donorsIndexService.Search(query).Result;

            return result;
        }
    }

    public SearchResult<ImageIndex> SearchImages(int donorId, ImageType type, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.Donor = new DonorCriteria { Id = new[] { donorId } };

        var criteriaFilters = GetFiltersCollection(type, criteria)
            .All();

        var query = new SearchQuery<ImageIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters);

        var result = _imagesIndexService.Search(query).Result;

        return result;
    }

    public SearchResult<SpecimenIndex> SearchSpecimens(int donorId, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.Donor = new DonorCriteria { Id = new[] { donorId } };

        var criteriaFilters = new SpecimenIndexFiltersCollection(criteria)
            .All();

        var query = new SearchQuery<SpecimenIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(specimen => specimen.NumberOfGenes);

        var result = _specimensIndexService.Search(query).Result;

        return result;
    }

    public SearchResult<GeneIndex> SearchGenes(int specimenId, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.Specimen = new SpecimenCriteria { Id = [specimenId]};

        var criteriaFilters = new GeneIndexFiltersCollection(criteria)
            .All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(gene => gene.NumberOfDonors);

        var result = _genesIndexService.Search(query).Result;

        return result;
    }

    public SearchResult<VariantIndex> SearchVariants(int specimenId, VariantType type, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        criteria.Specimen = new SpecimenCriteria { Id = [specimenId]};

        var criteriaFilters = GetFiltersCollection(type, criteria)
            .All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(criteriaFilters)
            .AddOrdering(mutation => mutation.NumberOfDonors);

        var result = _variantsIndexService.Search(query).Result;

        return result;
    }


    private static FiltersCollection<ImageIndex> GetFiltersCollection(ImageType type, SearchCriteria criteria)
    {
        return type switch
        {
            ImageType.MRI => new MriImageIndexFiltersCollection(criteria),
            _ => new ImageIndexFiltersCollection(criteria)
        };
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
