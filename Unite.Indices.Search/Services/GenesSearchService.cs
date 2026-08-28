using Unite.Indices.Entities.Genes;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class GenesSearchService : SearchService<GeneIndex>
{
    public GenesSearchService(IElasticOptions options) : base(options)
    {
    }

    protected override GetQuery<GeneIndex> BuildGetQuery(string key)
    {
        return base.BuildGetQuery(key)
            .AddExclusion(gene => gene.Specimens);
    }
    
    protected override SearchCriteria BuildSearchCriteria(int id)
    {
        return new SearchCriteria
        {
            Gene = new GeneCriteria
            {
                Id = new ValuesCriteria<int>([ id ])
            }
        };
    }

    protected override IIndexService<GeneIndex> GetIndexService()
    {
        return _genesIndexService;
    }

    public override async Task<SearchResult<GeneIndex>> Search(PersonalSearchCriteria personalSearchCriteria)
    {
        var criteria = personalSearchCriteria?.SearchCriteria ?? new SearchCriteria();

        var specimensToExclude = new HashSet<string>();
        var genesToExclude = new HashSet<string>();

        PersonalizeDonorsCriteria(personalSearchCriteria);

        if (criteria.HasDonorFilters)
        {
            var exclusive = criteria.AreDonorFiltersNegative;

            var ids = await AggregateFromDonors(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<GeneIndex>();
        }


        if (criteria.HasImageFilters)
        {
            var exclusive = criteria.AreImageFiltersNegative;

            var ids = await AggregateFromImages(index => index.Specimens.First().Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<GeneIndex>();
        }


        if (criteria.HasSpecimenFilters)
        {
            var exclusive = criteria.AreSpecimenFiltersNegative;

            var ids = await AggregateFromSpecimens(index => index.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<GeneIndex>();
        }


        if (specimensToExclude.Count > 0)
            criteria.Specimen = Set(criteria.Specimen, [.. specimensToExclude.Select(int.Parse)], true);


        if (criteria.HasProteinFilters)
        {
            var exclusive = criteria.AreProteinFiltersNegative;

            var ids = await AggregateFromProteins(index => index.Gene.Id, criteria, exclusive);

            if (HandleFoundGenes(exclusive, ids, ref genesToExclude, ref criteria))
                return new SearchResult<GeneIndex>();
        }


        if (criteria.HasSmFilters)
        {
            var exclusive = criteria.AreSmFiltersNegative;

            var ids = await AggregateFromSms(index => index.AffectedFeatures.First().Gene.Id, criteria, exclusive);

            if (HandleFoundGenes(exclusive, ids, ref genesToExclude, ref criteria))
                return new SearchResult<GeneIndex>();
        }

        if (criteria.HasCnvFilters)
        {
            var exclusive = criteria.AreCnvFiltersNegative;

            var ids = await AggregateFromCnvs(index => index.AffectedFeatures.First().Gene.Id, criteria, exclusive);

            if (HandleFoundGenes(exclusive, ids, ref genesToExclude, ref criteria))
                return new SearchResult<GeneIndex>();
        }

        if (criteria.HasSvFilters)
        {
            var exclusive = criteria.AreSvFiltersNegative;

            var ids = await AggregateFromSvs(index => index.AffectedFeatures.First().Gene.Id, criteria, exclusive);

            if (HandleFoundGenes(exclusive, ids, ref genesToExclude, ref criteria))
                return new SearchResult<GeneIndex>();
        }

        if (criteria.HasCnvProfileFilters)
        {
            var exclusive = criteria.AreCnvProfileFiltersNegative;

            var ids = await AggregateFromCnvProfiles(index => index.Specimen.Id, criteria, exclusive);

            if (HandleFoundSpecimens(exclusive, ids, ref specimensToExclude, ref criteria))
                return new SearchResult<GeneIndex>();
        }


        if (genesToExclude.Count > 0)
            criteria.Gene = Set(criteria.Gene, [.. genesToExclude.Select(int.Parse)], true);
        

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
