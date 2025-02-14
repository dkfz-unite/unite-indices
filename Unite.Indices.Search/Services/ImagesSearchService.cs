using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Images;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class ImagesSearchService : SearchService<ImageIndex>
{
    public ImagesSearchService(IElasticOptions options) : base(options)
    {
    }


    public override async Task<ImageIndex> Get(string key)
    {
        var query = new GetQuery<ImageIndex>(key);

        return await _imagesIndexService.Get(query);
    }

    public override async Task<SearchResult<ImageIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        if (criteria.HasDonorFilters)
        {
            var ids = await AggregateFromDonors(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Donor = Set(criteria.Donor, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ImageIndex>();

            if (criteria.Donor.Id.Length == 0)
                return new SearchResult<ImageIndex>();
        }

        if (criteria.HasSpecimenFilters)
        {
            var ids = await AggregateFromSpecimens(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ImageIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ImageIndex>();
        }

        if (criteria.HasGeneFilters && !criteria.HasVariantFilters)
        {
            var ids = await AggregateFromGenes(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ImageIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ImageIndex>();
        }

        if (criteria.HasSsmFilters)
        {
            var ids = await AggregateFromSsms(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ImageIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ImageIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var ids = await AggregateFromCnvs(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ImageIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ImageIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var ids = await AggregateFromSvs(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<ImageIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<ImageIndex>();
        }

        var filters = new ImageFiltersCollection(criteria).All();

        var query = new SearchQuery<ImageIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(image => image.Stats.Genes);

        return await _imagesIndexService.Search(query);
    }


    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, ImageIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
