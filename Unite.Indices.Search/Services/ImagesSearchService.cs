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

        var donorsToExclude = new HashSet<string>();
        var specimensToExclude = new HashSet<string>();
        var genesToExclude = new HashSet<string>();
        var proteinsToExclude = new HashSet<string>();


        if (criteria.HasDonorFilters)
        {
            var exclusive = criteria.AreDonorFiltersNegative;

            var ids = await AggregateFromDonors(index => index.Id, criteria, exclusive);

            if (HandleFoundDonors(exclusive, ids, ref donorsToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }

        if (donorsToExclude.Count > 0)
            criteria.Donor = Set(criteria.Donor, [.. donorsToExclude.Select(int.Parse)], true);


        if (criteria.HasSpecimenFilters)
        {
            var exclusive = criteria.AreSpecimenFiltersNegative;

            var ids = await AggregateFromSpecimens(index => index.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }


        if (criteria.HasGeneFilters)
        {
            var exclusive = criteria.AreGeneFiltersNegative;

            var specimenIds = await AggregateFromGenes(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<ImageIndex>();

            var gneIds = await AggregateFromGenes(index => index.Id, criteria, exclusive);

            if (HandleFoundGenes(exclusive, gneIds, ref genesToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }

        if (genesToExclude.Count > 0)
            criteria.Gene = Set(criteria.Gene, [.. genesToExclude.Select(int.Parse)], true);

        if (criteria.HasGeneExpressionFilters)
        {
            var exclusive = criteria.AreGeneFiltersNegative;

            var specimenIds = await AggregateFromGeneExpressions(index => index.Specimen.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }


        if (criteria.HasProteinFilters)
        {
            var exclusive = criteria.AreProteinFiltersNegative;

            var specimenIds = await AggregateFromProteins(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<ImageIndex>();

            var proteinIds = await AggregateFromProteins(index => index.Id, criteria, exclusive);

            if (HandleFoundProteins(exclusive, proteinIds, ref proteinsToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }

        if (proteinsToExclude.Count > 0)
            criteria.Protein = Set(criteria.Protein, [.. proteinsToExclude.Select(int.Parse)], true);

        if (criteria.HasProteinExpressionFilters)
        {
            var exclusive = criteria.AreProteinFiltersNegative;

            var specimenIds = await AggregateFromProteinExpressions(index => index.Specimen.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }


        if (criteria.HasSmFilters)
        {
            var exclusive = criteria.AreSmFiltersNegative;

            var ids = await AggregateFromSms(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var exclusive = criteria.AreCnvFiltersNegative;

            var ids = await AggregateFromCnvs(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var exclusive = criteria.AreSvFiltersNegative;

            var ids = await AggregateFromSvs(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }
        
        if (criteria.HasCnvProfileFilters)
        {
            var exclusive = criteria.AreCnvProfileFiltersNegative;

            var ids = await AggregateFromCnvProfiles(index => index.Specimen.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<ImageIndex>();
        }

        if (specimensToExclude.Count > 0)
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);
        

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
