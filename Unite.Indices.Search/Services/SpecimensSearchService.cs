using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Specimens;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class SpecimensSearchService : SearchService<SpecimenIndex>
{
    public SpecimensSearchService(IElasticOptions options) : base(options)
    {
    }


    public override async Task<SpecimenIndex> Get(string key)
    {
        var query = new GetQuery<SpecimenIndex>(key);

        return await _specimensIndexService.Get(query);
    }

    public override async Task<SearchResult<SpecimenIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        if (criteria.HasDonorFilters)
        {
            var ids = await AggregateFromDonors(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Donor = Set(criteria.Donor, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<SpecimenIndex>();

            if (criteria.Donor.Id.Length == 0)
                return new SearchResult<SpecimenIndex>();
        }

        if (criteria.HasImageFilters)
        {
            var ids = await AggregateFromImages(index => index.Id, criteria);

            if (ids.Length > 0)
                criteria.Image = Set(criteria.Image, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<SpecimenIndex>();

            if (criteria.Image.Id.Length == 0)
                return new SearchResult<SpecimenIndex>();
        }

        if (criteria.HasGeneFilters && !criteria.HasVariantFilters)
        {
            var ids = await AggregateFromGenes(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<SpecimenIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<SpecimenIndex>();
        }

        if (criteria.HasSsmFilters)
        {
            var ids = await AggregateFromSsms(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<SpecimenIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<SpecimenIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var ids = await AggregateFromCnvs(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<SpecimenIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<SpecimenIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var ids = await AggregateFromSvs(index => index.Specimens.First().Id, criteria);

            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return new SearchResult<SpecimenIndex>();

            if (criteria.Specimen.Id.Length == 0)
                return new SearchResult<SpecimenIndex>();
        }

        var filters = new SpecimenFiltersCollection(criteria).All();

        var query = new SearchQuery<SpecimenIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(specimen => specimen.Stats.Genes);

        return await _specimensIndexService.Search(query);
    }


    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, SpecimenIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
