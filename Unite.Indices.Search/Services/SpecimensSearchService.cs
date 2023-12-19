using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities.Specimens;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public class SpecimensSearchService : SearchService<SpecimenIndex>
{
    private readonly IIndexService<SpecimenIndex> _specimensIndexService;


    public SpecimensSearchService(IElasticOptions options) : base(options)
    {
        _specimensIndexService = new SpecimensIndexService(options);
    }


    public override async Task<SpecimenIndex> Get(string key)
    {
        var query = new GetQuery<SpecimenIndex>(key);

        return await _specimensIndexService.Get(query);
    }

    public override async Task<SearchResult<SpecimenIndex>> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        int[] ids = null;

        if(criteria.HasGeneFilters)
            ids = await AggregateFromGenes(index => index.Id, criteria);
        else if(criteria.HasVariantFilters)
            ids = await AggregateFromVariants(index => index.Id, criteria);

        if(ids != null)
        {
            if(ids.Length > 0)
                criteria.Specimen = (criteria.Specimen ?? new SpecimenCriteria()) with { Id = ids };
            else
                return new SearchResult<SpecimenIndex>();
        }

        var filters = new SpecimenFiltersCollection(criteria).All();

        var query = new SearchQuery<SpecimenIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(specimen => specimen.NumberOfGenes)
            .AddExclusion(specimen => specimen.Images)
            .AddExclusion(specimen => specimen.Cell.DrugScreenings)
            .AddExclusion(specimen => specimen.Organoid.DrugScreenings)
            .AddExclusion(specimen => specimen.Organoid.Interventions)
            .AddExclusion(specimen => specimen.Xenograft.DrugScreenings)
            .AddExclusion(specimen => specimen.Xenograft.Interventions);

        return await _specimensIndexService.Search(query);
    }


    protected override void AddToStats(ref Dictionary<object, Entities.DataIndex> stats, SpecimenIndex index)
    {
        stats.Add(index.Id, index.Data);
    }
}
