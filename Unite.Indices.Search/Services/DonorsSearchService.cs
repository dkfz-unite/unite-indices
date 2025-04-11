using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
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

        if (criteria.HasImageFilters)
        {
            var ids = await AggregateFromImages(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Image = Set(criteria.Image, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<DonorIndex>();

            if (criteria.Image.Id.Length == 0)
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasSpecimenFilters)
        { 
            var ids = await AggregateFromSpecimens(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<DonorIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasGeneFilters && !criteria.HasVariantFilters)
        {
            var ids = await AggregateFromGenes(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<DonorIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasSsmFilters)
        {
            var ids = await AggregateFromSms(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<DonorIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var ids = await AggregateFromCnvs(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<DonorIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var ids = await AggregateFromSvs(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<DonorIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<DonorIndex>();
        }

        var filters = new DonorFiltersCollection(criteria).All();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(donor => donor.Stats.Genes);

        return await _donorsIndexService.Search(query);
    }

    
    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, DonorIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
