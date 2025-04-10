using Unite.Indices.Entities.Genes;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class GenesSearchService : SearchService<GeneIndex>
{
    public GenesSearchService(IElasticOptions options) : base(options)
    {
    }


    public override async Task<GeneIndex> Get(string key)
    {
        var query = new GetQuery<GeneIndex>(key)
            .AddExclusion(gene => gene.Specimens);

        return await _genesIndexService.Get(query);
    }

    public override async Task<SearchResult<GeneIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        if (criteria.HasDonorFilters)
        {
            var ids = await AggregateFromDonors(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<GeneIndex>();
        }

        if (criteria.HasImageFilters)
        {
            var ids = await AggregateFromImages(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<GeneIndex>();
        }

        if (criteria.HasSpecimenFilters)
        {
            var ids = await AggregateFromSpecimens(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<GeneIndex>();
        }

        if (criteria.HasSsmFilters)
        {
            var ids = await AggregateFromSms(index => index.AffectedFeatures.First().Gene.Id, criteria);

            if (ids.Length > 0)
                criteria.Gene = Set(criteria.Gene, [.. ids.Select(int.Parse)]);

            if (criteria.Gene.Id.Length == 0)
                return new SearchResult<GeneIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var ids = await AggregateFromCnvs(index => index.AffectedFeatures.First().Gene.Id, criteria);

            if (ids.Length > 0)
                criteria.Gene = Set(criteria.Gene, [.. ids.Select(int.Parse)]);

            if (criteria.Gene.Id.Length == 0)
                return new SearchResult<GeneIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var ids = await AggregateFromSvs(index => index.AffectedFeatures.First().Gene.Id, criteria);

            if (ids.Length > 0)
                criteria.Gene = Set(criteria.Gene, [.. ids.Select(int.Parse)]);

            if (criteria.Gene.Id.Length == 0)
                return new SearchResult<GeneIndex>();
        }

        var filters = new GeneFiltersCollection(criteria).All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(gene => gene.Stats.Donors);

        return await _genesIndexService.Search(query);
    }


    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, GeneIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
