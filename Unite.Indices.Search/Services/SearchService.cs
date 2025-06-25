using System.Linq.Expressions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Genes.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Images.Criteria;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using SmIndex = Unite.Indices.Entities.Variants.SmIndex;
using CnvIndex = Unite.Indices.Entities.Variants.CnvIndex;
using SvIndex = Unite.Indices.Entities.Variants.SvIndex;


namespace Unite.Indices.Search.Services;


public abstract class SearchService<T> : ISearchService<T> where T : class
{
    protected readonly IIndexService<ProjectIndex> _projectsIndexService;
    protected readonly IIndexService<DonorIndex> _donorsIndexService;
    protected readonly IIndexService<ImageIndex> _imagesIndexService;
    protected readonly IIndexService<SpecimenIndex> _specimensIndexService;
    protected readonly IIndexService<GeneIndex> _genesIndexService;
    protected readonly IIndexService<SmIndex> _smsIndexService;
    protected readonly IIndexService<CnvIndex> _cnvsIndexService;
    protected readonly IIndexService<SvIndex> _svsIndexService;


    protected SearchService(IElasticOptions options)
    {
        _projectsIndexService = new ProjectsIndexService(options);
        _donorsIndexService = new DonorsIndexService(options);
        _imagesIndexService = new ImagesIndexService(options);
        _specimensIndexService = new SpecimensIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _smsIndexService = new SmsIndexService(options);
        _cnvsIndexService = new CnvsIndexService(options);
        _svsIndexService = new SvsIndexService(options);
    }


    public abstract Task<T> Get(string key);

    public abstract Task<SearchResult<T>> Search(SearchCriteria searchCriteria);

    public virtual async Task<IReadOnlyDictionary<object, DataIndex>> Stats(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria with { From = 0, Size = 0 };

        var lookupResult = await Search(criteria);

        var availableData = new Dictionary<object, DataIndex>();

        for (var from = 0; from < lookupResult.Total; from += 499)
        {
            criteria = criteria with { From = from, Size = 499 };

            var searchResult = await Search(criteria);

            foreach (var index in searchResult.Rows)
            {
                AddToStats(ref availableData, index);
            }
        }

        return availableData.AsReadOnly();
    }


    protected abstract void AddToStats(ref Dictionary<object, DataIndex> stats, T index);

    protected async Task<string[]> AggregateFromDonors<TProp>(Expression<Func<DonorIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new DonorFiltersCollection(criteria);

        var aggregation = await AggregateFromDonors(property, criteria.Term, filters);

        return aggregation.Keys.ToArray();
    }

    public async Task<string[]> AggregateFromImages<TProp>(Expression<Func<ImageIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new ImageFiltersCollection(criteria);

        var aggregation = await AggregateFromImages(property, criteria.Term, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromSpecimens<TProp>(Expression<Func<SpecimenIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new SpecimenFiltersCollection(criteria);

        var aggregation = await AggregateFromSpecimens(property, criteria.Term, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new GeneFiltersCollection(criteria);

        var aggregation = await AggregateFromGenes(property, criteria.Term, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromSms<TProp>(Expression<Func<SmIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new SmFiltersCollection(criteria);

        var aggregation = await AggregateFromSms(property, criteria.Term, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromCnvs<TProp>(Expression<Func<CnvIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new CnvFiltersCollection(criteria);

        var aggregation = await AggregateFromCnvs(property, criteria.Term, filters);

        return aggregation.Keys.ToArray();
    }

    protected async Task<string[]> AggregateFromSvs<TProp>(Expression<Func<SvIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new SvFiltersCollection(criteria);

        var aggregation = await AggregateFromSvs(property, criteria.Term, filters);

        return aggregation.Keys.ToArray();
    }

    protected static DonorCriteria Set(DonorCriteria criteria, int[] ids)
    {
        return (criteria ?? new DonorCriteria()) with { Id = new ValuesCriteria<int>(Intersect(criteria?.Id.Value, ids)) };
    }

    protected static ImagesCriteria Set(ImagesCriteria criteria, int[] ids)
    {
        return (criteria ?? new ImagesCriteria()) with { Id = new ValuesCriteria<int>(Intersect(criteria?.Id.Value, ids)) };
    }

    protected static SpecimensCriteria Set(SpecimensCriteria criteria, int[] ids)
    {
        return (criteria ?? new SpecimensCriteria()) with { Id = new ValuesCriteria<int>(Intersect(criteria?.Id.Value, ids)) };
    }

    protected static GeneCriteria Set(GeneCriteria criteria, int[] ids)
    {
        return (criteria ?? new GeneCriteria()) with { Id = new ValuesCriteria<int>(Intersect(criteria?.Id.Value, ids)) };
    }


    private async Task<IDictionary<string, long>> AggregateFromDonors<TProp>(Expression<Func<DonorIndex, TProp>> property, string term, DonorFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<DonorIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Images)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _donorsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromImages<TProp>(Expression<Func<ImageIndex, TProp>> property, string term, ImageFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<ImageIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Donor)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _imagesIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromSpecimens<TProp>(Expression<Func<SpecimenIndex, TProp>> property, string term, SpecimenFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<SpecimenIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Donor)
            .AddExclusion(index => index.Images)
            .AddExclusion(index => index.Samples)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _specimensIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, string term, GeneFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _genesIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromSms<TProp>(Expression<Func<SmIndex, TProp>> property, string term, SmFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<SmIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _smsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromCnvs<TProp>(Expression<Func<CnvIndex, TProp>> property, string term, CnvFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<CnvIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _cnvsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromSvs<TProp>(Expression<Func<SvIndex, TProp>> property, string term, SvFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<SvIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Stats)
            .AddExclusion(index => index.Data);

        var result = await _svsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }
    
    private static int[] Intersect(int[] a, int[] b)
    {        
        if (a == null || a.Length == 0)
            return b;
        else
            return a.Intersect(b).ToArray();
    }
}
