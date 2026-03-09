using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities;
using Unite.Indices.Entities.Proteins;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class ProteinsSearchService : SearchService<ProteinIndex>
{
    public ProteinsSearchService(IElasticOptions options) : base(options)
    {
    }


    public override Task<ProteinIndex> Get(string key)
    {
        var query = new GetQuery<ProteinIndex>(key)
            .AddExclusion(protein => protein.Specimens);

        return _proteinsIndexService.Get(query);
    }

    public override async Task<SearchResult<ProteinIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var specimensToExclude = new HashSet<string>();
        var proteinsToExclude = new HashSet<string>();


        if (criteria.HasDonorFilters)
        {
            var exclusive = criteria.AreDonorFiltersNegative;

            var ids = await AggregateFromDonors(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<ProteinIndex>();
        }


        if (criteria.HasImageFilters)
        {
            var exclusive = criteria.AreImageFiltersNegative;

            var ids = await AggregateFromImages(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<ProteinIndex>();
        }


        if (criteria.HasSpecimenFilters)
        {
            var exclusive = criteria.AreSpecimenFiltersNegative;

            var ids = await AggregateFromSpecimens(index => index.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<ProteinIndex>();
        }


        if (specimensToExclude.Count > 0)
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);


        // TODO: Add gene expression filters integration


        if (criteria.HasSmFilters)
        {
            var exclusive = criteria.AreSmFiltersNegative;

            var ids = await AggregateFromSms(index => index.AffectedFeatures.First().Transcript.Feature.Protein.Id, criteria, exclusive);

            if (HandleFoundProteins(exclusive, ids, ref proteinsToExclude, ref criteria))
                return new SearchResult<ProteinIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var exclusive = criteria.AreCnvFiltersNegative;

            var ids = await AggregateFromCnvs(index => index.AffectedFeatures.First().Transcript.Feature.Protein.Id, criteria, exclusive);

            if (HandleFoundProteins(exclusive, ids, ref proteinsToExclude, ref criteria))
                return new SearchResult<ProteinIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var exclusive = criteria.AreSvFiltersNegative;

            var ids = await AggregateFromSvs(index => index.AffectedFeatures.First().Transcript.Feature.Protein.Id, criteria, exclusive);

            if (HandleFoundProteins(exclusive, ids, ref proteinsToExclude, ref criteria))
                return new SearchResult<ProteinIndex>();
        }

        if (proteinsToExclude.Count > 0)
            criteria.Protein = Set(criteria.Protein, [.. proteinsToExclude.Select(int.Parse)], true);


        var filters = new ProteinFiltersCollection(criteria).All();

        var query = new SearchQuery<ProteinIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(gene => gene.Stats.Donors);

        return await _proteinsIndexService.Search(query);
    }


    protected override void AddToStats(ref Dictionary<object, DataIndex> stats, ProteinIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
