using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

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


    public ImageIndex Get(string key)
    {
        var query = new GetQuery<ImageIndex>(key)
            .AddExclusion(image => image.Donor)
            .AddExclusion(image => image.Specimens.First().Tissue)
            .AddExclusion(image => image.Specimens.First().Cell)
            .AddExclusion(image => image.Specimens.First().Organoid)
            .AddExclusion(image => image.Specimens.First().Xenograft);

        return _imagesIndexService.Get(query).Result;
    }

    public SearchResult<ImageIndex> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        int[] ids = null;

        if (criteria.HasGeneFilters)
            ids = AggregateFromGenes(index => index.Specimens.First().Images.First().Id, criteria);
        else if (criteria.HasVariantFilters)
            ids = AggregateFromVariants(index => index.Specimens.First().Images.First().Id, criteria);

        if (ids != null)
        {
            if (ids.Length > 0)
                criteria.Image = (criteria.Image ?? new ImageCriteria()) with { Id = ids };
            else
                return new SearchResult<ImageIndex>();
        }

        var filters = new ImageFiltersCollection(criteria).All();

        var query = new SearchQuery<ImageIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(image => image.Id, true)
            .AddExclusion(image => image.Specimens);

        return _imagesIndexService.Search(query).Result;
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

    public SearchResult<VariantIndex> SearchVariants(SearchCriteria searchCriteria)
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
}
