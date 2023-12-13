using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Specimens.DataIndex;


namespace Unite.Indices.Search.Services;

public class SpecimensSearchService : AggregatingSearchService, ISpecimensSearchService
{
    private readonly IIndexService<SpecimenIndex> _specimensIndexService;
    private readonly IIndexService<GeneIndex> _genesIndexService;
    private readonly IIndexService<VariantIndex> _variantsIndexService;

    public override IIndexService<GeneIndex> GenesIndexService => _genesIndexService;
    public override IIndexService<VariantIndex> VariantsIndexService => _variantsIndexService;


    public SpecimensSearchService(IElasticOptions options)
    {
        _specimensIndexService = new SpecimensIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }


    public SpecimenIndex Get(string key)
    {
        var query = new GetQuery<SpecimenIndex>(key);

        return _specimensIndexService.Get(query).Result;
    }

    public SearchResult<SpecimenIndex> Search(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        int[] ids = null;

        if(criteria.HasGeneFilters)
            ids = AggregateFromGenes(index => index.Id, criteria);
        else if(criteria.HasVariantFilters)
            ids = AggregateFromVariants(index => index.Id, criteria);

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
            .AddExclusion(specimen => specimen.Cell.DrugScreenings)
            .AddExclusion(specimen => specimen.Organoid.DrugScreenings)
            .AddExclusion(specimen => specimen.Xenograft.DrugScreenings)
            .AddExclusion(specimen => specimen.Images);

        return _specimensIndexService.Search(query).Result;
    }

    public SearchResult<GeneIndex> SearchGenes(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var filters = new GeneFiltersCollection(criteria).All();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(gene => gene.NumberOfDonors);

        return _genesIndexService.Search(query).Result;
    }

    public SearchResult<VariantIndex> SearchVariants(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var filters = new VariantFiltersCollection(criteria).All();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(criteria.From, criteria.Size)
            .AddFullTextSearch(criteria.Term)
            .AddFilters(filters)
            .AddOrdering(mutation => mutation.NumberOfDonors);

        return _variantsIndexService.Search(query).Result;
    }

    public IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria;

        var availableData = new Dictionary<int, DataIndex>();

        criteria = criteria with { From = 0, Size = 0 };

        var lookupResult = Search(criteria);

        for (var from = 0; from < lookupResult.Total; from += 499)
        {
            criteria = criteria with { From = from, Size = 499 };

            var searchResult = Search(criteria);

            foreach (var index in searchResult.Rows)
            {
                availableData.Add(index.Id, index.Data);
            }
        }

        return availableData;
    }
}
