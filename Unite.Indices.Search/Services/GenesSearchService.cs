using Unite.Indices.Entities.Genes;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;
using Unite.Essentials.Extensions;

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
                    return new SearchResult<GeneIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<GeneIndex>();
            }
        }

        if (criteria.HasImageFilters)
        {
            var exclusive = criteria.AreImageFiltersNegative;

            var ids = await AggregateFromImages(index => index.Specimens.First().Id, criteria, exclusive);

            if (exclusive)
            {
                specimensToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Specimen = Set(criteria.Specimen, [.. ids.Select(int.Parse)]);
                else if (!exclusive)
                    return new SearchResult<GeneIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<GeneIndex>();
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
                    return new SearchResult<GeneIndex>();

                if (criteria.Specimen.Id.Length == 0)
                    return new SearchResult<GeneIndex>();
            }
        }

        if (specimensToExclude.Count > 0)
        {
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);
        }


        var genesToExclude = new HashSet<string>();

        if (criteria.HasSmFilters)
        {
            var exclusive = criteria.AreSmFiltersNegative;

            var ids = await AggregateFromSms(index => index.AffectedFeatures.First().Gene.Id, criteria, exclusive);

            if (exclusive)
            {
                genesToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Gene = Set(criteria.Gene, [.. ids.Select(int.Parse)]);
                else if (!exclusive)
                    return new SearchResult<GeneIndex>();

                if (criteria.Gene.Id.Length == 0)
                    return new SearchResult<GeneIndex>();
            }
        }

        if (criteria.HasCnvFilters)
        {
            var exclusive = criteria.AreCnvFiltersNegative;

            var ids = await AggregateFromCnvs(index => index.AffectedFeatures.First().Gene.Id, criteria, exclusive);

            if (exclusive)
            {
                genesToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Gene = Set(criteria.Gene, [.. ids.Select(int.Parse)]);
                else if (!exclusive)
                    return new SearchResult<GeneIndex>();

                if (criteria.Gene.Id.Length == 0)
                    return new SearchResult<GeneIndex>();
            }
        }

        if (criteria.HasSvFilters)
        {
            var exclusive = criteria.AreSvFiltersNegative;

            var ids = await AggregateFromSvs(index => index.AffectedFeatures.First().Gene.Id, criteria, exclusive);

            if (exclusive)
            {
                genesToExclude.AddRange(ids);
            }
            else
            {
                if (ids.Length > 0)
                    criteria.Gene = Set(criteria.Gene, [.. ids.Select(int.Parse)]);
                else if (!exclusive)
                    return new SearchResult<GeneIndex>();

                if (criteria.Gene.Id.Length == 0)
                    return new SearchResult<GeneIndex>();
            }
        }

        if (genesToExclude.Count > 0)
        {
            criteria.Gene = Set(criteria.Gene, [.. genesToExclude.Select(int.Parse)], true);
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
