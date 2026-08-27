using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Variants;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Variants.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class SmsSearchService : SearchService<SmIndex>
{
    public SmsSearchService(IElasticOptions options) : base(options)
    {
    }

    protected override GetQuery<SmIndex> BuildGetQuery(string key)
    {
        return base.BuildGetQuery(key)
            .AddExclusion(variant => variant.Specimens);
    }

    protected override void BuildSearchCriteria(SearchCriteria searchCriteria, int id)
    {
        searchCriteria.Sm = new SmCriteria
        {
            Id = new ValuesCriteria<int>([ id ])
        };
    }

    protected override IIndexService<SmIndex> GetIndexService()
    {
        return _smsIndexService;
    }

    public override async Task<SearchResult<SmIndex>> Search(PersonalSearchCriteria personalSearchCriteria)
    {
        var criteria = personalSearchCriteria.SearchCriteria;

        var specimensToExclude = new HashSet<string>();
        var genesToExclude = new HashSet<string>();
        var proteinsToExclude = new HashSet<string>();
        
        PersonalizeDonorsCriteria(personalSearchCriteria);

        if (criteria.HasDonorFilters)
        {
            var exclusive = criteria.AreDonorFiltersNegative;

            var ids = await AggregateFromDonors(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<SmIndex>();
        }


        if (criteria.HasImageFilters)
        {
            var exclusive = criteria.AreImageFiltersNegative;

            var ids = await AggregateFromImages(index => index.Specimens.First().Id, criteria with { Specimen = null }, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<SmIndex>();
        }


        if (criteria.HasSpecimenFilters)
        {
            var exclusive = criteria.AreSpecimenFiltersNegative;

            var ids = await AggregateFromSpecimens(index => index.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<SmIndex>();
        }


        if (specimensToExclude.Count > 0)
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);
        

        if (criteria.HasGeneFilters)
        {
            var exclusive = criteria.AreGeneFiltersNegative;

            var specimenIds = await AggregateFromGenes(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<SmIndex>();

            var gneIds = await AggregateFromGenes(index => index.Id, criteria, exclusive);

            if (HandleFoundGenes(exclusive, gneIds, ref genesToExclude, ref criteria))
                return new SearchResult<SmIndex>();
        }

        if (genesToExclude.Count > 0)
            criteria.Gene = Set(criteria.Gene, [.. genesToExclude.Select(int.Parse)], true);

        if (criteria.HasGeneExpressionFilters)
        {
            var exclusive = criteria.AreGeneFiltersNegative;

            var specimenIds = await AggregateFromGeneExpressions(index => index.Specimen.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<SmIndex>();
        }


        if (criteria.HasProteinFilters)
        {
            var exclusive = criteria.AreProteinFiltersNegative;

            var specimenIds = await AggregateFromProteins(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<SmIndex>();

            var proteinIds = await AggregateFromProteins(index => index.Id, criteria, exclusive);

            if (HandleFoundProteins(exclusive, proteinIds, ref proteinsToExclude, ref criteria))
                return new SearchResult<SmIndex>();
        }

        if (proteinsToExclude.Count > 0)
            criteria.Protein = Set(criteria.Protein, [.. proteinsToExclude.Select(int.Parse)], true);

        if (criteria.HasProteinExpressionFilters)
        {
            var exclusive = criteria.AreProteinFiltersNegative;

            var specimenIds = await AggregateFromProteinExpressions(index => index.Specimen.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, specimenIds, ref specimensToExclude, ref criteria))
                return new SearchResult<SmIndex>();
        }


        var filters = new SmFiltersCollection(criteria).All();

        var query = new SearchQuery<SmIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(variant => variant.Stats.Donors)
            .AddOrdering(variant => variant.ChromosomeI, true)
            .AddOrdering(variant => variant.Start, true);

        return await _smsIndexService.Search(query);
    }

    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, SmIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
