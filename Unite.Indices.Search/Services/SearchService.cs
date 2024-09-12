using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Indices.Search.Services;


public abstract class SearchService<T> : ISearchService<T> where T : class
{
    protected readonly IIndexService<DonorIndex> _donorsIndexService;
    protected readonly IIndexService<ImageIndex> _imagesIndexService;
    protected readonly IIndexService<SpecimenIndex> _specimensIndexService;
    protected readonly IIndexService<GeneIndex> _genesIndexService;
    protected readonly IIndexService<VariantIndex> _variantsIndexService;


    protected SearchService(IElasticOptions options)
    {
        _donorsIndexService = new DonorsIndexService(options);
        _imagesIndexService = new ImagesIndexService(options);
        _specimensIndexService = new SpecimensIndexService(options);
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
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

    protected async Task<int[]> AggregateFromDonors<TProp>(Expression<Func<DonorIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new DonorFiltersCollection(criteria);

        var aggregation = await AggregateFromDonors(property, criteria.Term, filters);

        return aggregation.Select(x => int.Parse(x.Key)).ToArrayOrNull();
    }

    public async Task<int[]> AggregateFromImages<TProp>(Expression<Func<ImageIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new ImageFiltersCollection(criteria);

        var aggregation = await AggregateFromImages(property, criteria.Term, filters);

        return aggregation.Select(x => int.Parse(x.Key)).ToArrayOrNull();
    }

    protected async Task<int[]> AggregateFromSpecimens<TProp>(Expression<Func<SpecimenIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new SpecimenFiltersCollection(criteria);

        var aggregation = await AggregateFromSpecimens(property, criteria.Term, filters);

        return aggregation.Select(x => int.Parse(x.Key)).ToArrayOrNull();
    }

    protected async Task<int[]> AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new GeneFiltersCollection(criteria);

        var aggregation = await AggregateFromGenes(property, criteria.Term, filters);

        return aggregation.Select(x => int.Parse(x.Key)).ToArrayOrNull();
    }

    protected async Task<int[]> AggregateFromVariants<TProp>(Expression<Func<VariantIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new VariantFiltersCollection(criteria);

        var aggregation = await AggregateFromVariants(property, criteria.Term, filters);

        return aggregation.Select(x => int.Parse(x.Key)).ToArrayOrNull();
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
            .AddExclusion(index => index.Specimens);

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
            .AddExclusion(index => index.Specimens);

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
            .AddExclusion(index => index.Samples);

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
            .AddExclusion(index => index.Data);

        var result = await _genesIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }

    private async Task<IDictionary<string, long>> AggregateFromVariants<TProp>(Expression<Func<VariantIndex, TProp>> property, string term, VariantFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Data);

        var result = await _variantsIndexService.Search(query);

        return result.Aggregations[aggregationName];
    }
}
