using Unite.Essentials.Extensions;
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

        var imagesToExclude = new HashSet<string>();

        if (criteria.HasImageFilters)
        {
            var exclusive = criteria.AreImageFiltersNegative;

            var ids = await AggregateFromImages(index => index.Id, criteria, exclusive);

            var stop = HandleFoundImages(exclusive, ids, ref imagesToExclude, ref criteria);

            if (stop)
                return new SearchResult<DonorIndex>();
        }

        if (imagesToExclude.Count > 0)
        {
            criteria.Image = Set(criteria.Image, [.. imagesToExclude.Select(int.Parse)], true);
        }


        var specimensToExclude = new HashSet<string>();

        if (criteria.HasSpecimenFilters)
        {
            var exclusive = criteria.AreSpecimenFiltersNegative;

            var ids = await AggregateFromSpecimens(index => index.Id, criteria, exclusive);

            var stop = HandleFoudSpecimens(exclusive, ids, ref specimensToExclude, ref criteria);

            if (stop)
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasGeneFilters && !criteria.HasVariantFilters)
        {
            var exclusive = criteria.AreGeneFiltersNegative;

            var ids = await AggregateFromGenes(index => index.Specimens.First().Id, criteria, exclusive);

            var stop = HandleFoudSpecimens(exclusive, ids, ref specimensToExclude, ref criteria);

            if (stop)
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasSmFilters)
        {
            var exclusive = criteria.AreSmFiltersNegative;

            var ids = await AggregateFromSms(index => index.Specimens.First().Id, criteria, exclusive);

            var stop = HandleFoudSpecimens(exclusive, ids, ref specimensToExclude, ref criteria);

            if (stop)
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var exclusive = criteria.AreCnvFiltersNegative;

            var ids = await AggregateFromCnvs(index => index.Specimens.First().Id, criteria, exclusive);

            var stop = HandleFoudSpecimens(exclusive, ids, ref specimensToExclude, ref criteria);

            if (stop)
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var exclusive = criteria.AreSvFiltersNegative;

            var ids = await AggregateFromSvs(index => index.Specimens.First().Id, criteria, exclusive);

            var stop = HandleFoudSpecimens(exclusive, ids, ref specimensToExclude, ref criteria);

            if (stop)
                return new SearchResult<DonorIndex>();
        }

        if (specimensToExclude.Count > 0)
        {
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);
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

    private static bool HandleFoundImages(in bool exclusive, in string[] ids, ref HashSet<string> idsToExclude, ref SearchCriteria criteria)
    {
        if (exclusive)
        {
            idsToExclude.AddRange(ids);
        }
        else
        {
            if (ids.Length > 0)
                criteria.Image = Set(criteria.Image, [.. ids.Select(int.Parse)]);
            else
                return true;

            if (criteria.Image.Id.Length == 0)
                return true;
        }

        return false;
    }

    private static bool HandleFoudSpecimens(in bool exclusive, in string[] ids, ref HashSet<string> idsToExclude, ref SearchCriteria criteria)
    {
        if (exclusive)
        {
            idsToExclude.AddRange(ids);
        }
        else
        {
            if (ids.Length > 0)
                criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
            else
                return true;

            if (criteria.Specimen.Id.Length == 0)
                return true;
        }

        return false;
    }
}
