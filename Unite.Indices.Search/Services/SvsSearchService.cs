using Unite.Essentials.Extensions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class SvsSearchService : SearchService<SvIndex>
{
    public SvsSearchService(IElasticOptions options) : base(options)
    {
    }

    public override async Task<SvIndex> Get(string key)
    {
        var query = new GetQuery<SvIndex>(key)
            .AddExclusion(variant => variant.Specimens);

        return await _svsIndexService.Get(query);
    }

    public override async Task<SearchResult<SvIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var specimensToExclude = new HashSet<string>();

        if (criteria.HasDonorFilters)
        {
            var exclusive = criteria.AreDonorFiltersNegative;

            var ids = await AggregateFromDonors(index => index.Specimens.First().Id, criteria, exclusive);

            if (exclusive)
            {
                specimensToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
                else if (!exclusive)
                    return new SearchResult<SvIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<SvIndex>();
            }
        }

        if (criteria.HasImageFilters)
        {
            var exclusive = criteria.AreImageFiltersNegative;

            var ids = await AggregateFromImages(index => index.Specimens.First().Id, criteria with { Specimen = null }, exclusive);

            if (exclusive)
            {
                specimensToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
                else if (!exclusive)
                    return new SearchResult<SvIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<SvIndex>();
            }
        }

        if (criteria.HasSpecimenFilters)
        {
            var exclusive = criteria.AreSpecimenFiltersNegative;

            var ids = await AggregateFromSpecimens(index => index.Id, criteria, exclusive);

            if (exclusive)
            {
                specimensToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
                else if (!exclusive)
                    return new SearchResult<SvIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<SvIndex>();
            }
        }

        if (specimensToExclude.Count > 0)
        {
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);
        }
        

        var filters = new SvFiltersCollection(criteria).All();

        var query = new SearchQuery<SvIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(variant => variant.Stats.Donors)
            .AddOrdering(variant => variant.ChromosomeI, true)
            .AddOrdering(variant => variant.Start, true);

        return await _svsIndexService.Search(query);
    }

    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, SvIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
