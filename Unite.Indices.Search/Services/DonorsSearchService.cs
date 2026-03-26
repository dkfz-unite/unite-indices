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
        var specimensToExclude = new HashSet<string>();
        var genesToExclude = new HashSet<string>();
        var proteinsToExclude = new HashSet<string>();


        if (criteria.HasImageFilters)
        {
            var exclusive = criteria.AreImageFiltersNegative;

            var ids = await AggregateFromImages(index => index.Id, criteria, exclusive);

            if (HandleFoundImages(exclusive, ids, ref imagesToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }

        if (imagesToExclude.Count > 0)
            criteria.Image = Set(criteria.Image, [.. imagesToExclude.Select(int.Parse)], true);


        if (criteria.HasSpecimenFilters)
        {
            var exclusive = criteria.AreSpecimenFiltersNegative;

            var ids = await AggregateFromSpecimens(index => index.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }


        if (criteria.HasGeneFilters)
        {
            var exclusive = criteria.AreGeneFiltersNegative;

            var specimenIds = await AggregateFromGenes(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<DonorIndex>();

            var gneIds = await AggregateFromGenes(index => index.Id, criteria, exclusive);

            if (HandleFoundGenes(exclusive, gneIds, ref genesToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }
        
        if (genesToExclude.Count > 0)
            criteria.Gene = Set(criteria.Gene, [.. genesToExclude.Select(int.Parse)], true);

        if (criteria.HasGeneExpressionFilters)
        {
            var exclusive = criteria.AreGeneFiltersNegative;

            var specimenIds = await AggregateFromGeneExpressions(index => index.Specimen.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }


        if (criteria.HasProteinFilters)
        {
            var exclusive = criteria.AreProteinFiltersNegative;

            var specimenIds = await AggregateFromProteins(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<DonorIndex>();

            var proteinIds = await AggregateFromProteins(index => index.Id, criteria, exclusive);

            if (HandleFoundProteins(exclusive, proteinIds, ref proteinsToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }

        if (proteinsToExclude.Count > 0)
            criteria.Protein = Set(criteria.Protein, [.. proteinsToExclude.Select(int.Parse)], true);

        if (criteria.HasProteinExpressionFilters)
        {
            var exclusive = criteria.AreProteinFiltersNegative;

            var specimenIds = await AggregateFromProteinExpressions(index => index.Specimen.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }


        if (criteria.HasSmFilters)
        {
            var exclusive = criteria.AreSmFiltersNegative;

            var ids = await AggregateFromSms(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var exclusive = criteria.AreCnvFiltersNegative;

            var ids = await AggregateFromCnvs(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var exclusive = criteria.AreSvFiltersNegative;

            var ids = await AggregateFromSvs(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }
        
        if (criteria.HasCnvProfileFilters)
        {
            var exclusive = criteria.AreCnvProfileFiltersNegative;

            var ids = await AggregateFromCnvProfiles(index => index.Specimen.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<DonorIndex>();
        }

        if (specimensToExclude.Count > 0)
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);


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
