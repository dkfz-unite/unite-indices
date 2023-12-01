using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Enums;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Context;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Images.DataIndex;


namespace Unite.Indices.Search.Services;

public class ImagesSearchService : AggregatingSearchService, IImagesSearchService
{
    private readonly IIndexService<ImageIndex> _imagesIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public override IIndexService<GeneIndex> GenesIndexService => _genesIndexService;
    public override IIndexService<VariantIndex> VariantsIndexService => _variantsIndexService;


    public ImagesSearchService(IElasticOptions options)
    {
        _imagesIndexService = new ImagesIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }


    public ImageIndex Get(string key, ImageSearchContext searchContext = null)
    {
        var query = new GetQuery<ImageIndex>(key)
            .AddExclusion(image => image.Donor)
            .AddExclusion(image => image.Specimens.First().Tissue)
            .AddExclusion(image => image.Specimens.First().Cell)
            .AddExclusion(image => image.Specimens.First().Organoid)
            .AddExclusion(image => image.Specimens.First().Xenograft);

        var result = _imagesIndexService.Get(query).Result;

        return result;
    }

    public IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        var context = searchContext ?? new ImageSearchContext();

        var availableData = new Dictionary<int, DataIndex>();

        criteria = criteria with { From = 0, Size = 0 };
        var lookupResult = Search(criteria, context);

        for (var from = 0; from < lookupResult.Total; from += 499)
        {
            criteria = criteria with { From = from, Size = 499 };
            var searchResult = Search(criteria, context);

            foreach (var index in searchResult.Rows)
            {
                availableData.Add(index.Id, index.Data);
            }
        }

        return availableData;
    }

    public SearchResult<ImageIndex> Search(SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new ImageSearchContext();

        var filters = GetFiltersCollection(criteria, context)
            .All();

        var ids = AggregateFromVariants(index => index.Specimens.First().Images.First().Id, criteria)
               ?? AggregateFromGenes(index => index.Specimens.First().Images.First().Id, criteria)
               ?? null;

        if (ids?.Length == 0)
        {
            return new SearchResult<ImageIndex>();
        }
        else
        {
            if (ids != null)
            {
                criteria.Image = (criteria.Image ?? new ImageCriteria()) with { Id = ids };
            }

            var query = new SearchQuery<ImageIndex>()
                .AddPagination(criteria.From, criteria.Size)
                .AddFullTextSearch(criteria.Term)
                .AddFilters(filters)
                .AddOrdering(image => image.Id, true)
                .AddExclusion(image => image.Specimens);

            var result = _imagesIndexService.Search(query).Result;

            return result;
        }
    }

    public SearchResult<GeneIndex> SearchGenes(int specimenId, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new ImageSearchContext();

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

    public SearchResult<VariantIndex> SearchVariants(int specimenId, VariantType type, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var context = searchContext ?? new ImageSearchContext();

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


    private static FiltersCollection<ImageIndex> GetFiltersCollection(SearchCriteria criteria, ImageSearchContext context)
    {
        return context.ImageType switch
        {
            ImageType.MRI => new MriImageIndexFiltersCollection(criteria),
            ImageType.CT => throw new NotImplementedException(),
            _ => new ImageIndexFiltersCollection(criteria),
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
