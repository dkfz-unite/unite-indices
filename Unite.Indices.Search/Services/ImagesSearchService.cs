using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Images;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class ImagesSearchService : SearchService<ImageIndex>
{
    private readonly IIndexService<ImageIndex> _imagesIndexService;


    public ImagesSearchService(IElasticOptions options) : base(options)
    {
        _imagesIndexService = new ImagesIndexService(options);
    }


    public override async Task<ImageIndex> Get(string key)
    {
        var query = new GetQuery<ImageIndex>(key);

        return await _imagesIndexService.Get(query);
    }

    public override async Task<SearchResult<ImageIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        int[] ids = null;

        if (criteria.HasVariantFilters)
            ids = await AggregateFromVariants(index => index.Specimens.First().Images.First().Id, criteria) ?? [];
        else if (criteria.HasGeneFilters)
            ids = await AggregateFromGenes(index => index.Specimens.First().Images.First().Id, criteria) ?? [];

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
            .AddOrdering(image => image.NumberOfGenes)
            .AddExclusion(image => image.Specimens.First().Material)
            .AddExclusion(image => image.Specimens.First().Line)
            .AddExclusion(image => image.Specimens.First().Organoid)
            .AddExclusion(image => image.Specimens.First().Xenograft);

        return await _imagesIndexService.Search(query);
    }


    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, ImageIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
