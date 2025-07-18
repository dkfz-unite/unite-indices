using Unite.Essentials.Extensions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class CnvsSearchService : SearchService<CnvIndex>
{
    public CnvsSearchService(IElasticOptions options) : base(options)
    {
    }

    public override async Task<CnvIndex> Get(string key)
    {
        var query = new GetQuery<CnvIndex>(key)
            .AddExclusion(variant => variant.Specimens);

        return await _cnvsIndexService.Get(query);
    }

    public override async Task<SearchResult<CnvIndex>> Search(SearchCriteria searchCriteria)
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
                    return new SearchResult<CnvIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<CnvIndex>();
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
                    return new SearchResult<CnvIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<CnvIndex>();
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
                    return new SearchResult<CnvIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<CnvIndex>();
            }
        }

        if (specimensToExclude.Count > 0)
        {
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);
        }
        

        var filters = new CnvFiltersCollection(criteria).All();

        var query = new SearchQuery<CnvIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(variant => variant.Stats.Donors)
            .AddOrdering(variant => variant.ChromosomeI, true)
            .AddOrdering(variant => variant.Start, true);

        return await _cnvsIndexService.Search(query);
    }

    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, CnvIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
