using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Indices.Search.Services;


public abstract class SearchService<T> : ISearchService<T> where T : class
{
    protected readonly IIndexService<GeneIndex> _genesIndexService;
    protected readonly IIndexService<VariantIndex> _variantsIndexService;


    protected SearchService(IElasticOptions options)
    {
        _genesIndexService = new GenesIndexService(options);
        _variantsIndexService = new VariantsIndexService(options);
    }


    public abstract T Get(string key);

    public abstract SearchResult<T> Search(SearchCriteria searchCriteria);

    public virtual IReadOnlyDictionary<object, DataIndex> Stats(SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria with { From = 0, Size = 0 };

        var lookupResult = Search(criteria);

        var availableData = new Dictionary<object, DataIndex>();

        for (var from = 0; from < lookupResult.Total; from += 499)
        {
            criteria = criteria with { From = from, Size = 499 };

            var searchResult = Search(criteria);

            foreach (var index in searchResult.Rows)
            {
                AddToStats(ref availableData, index);
            }
        }

        return availableData.AsReadOnly();
    }


    protected abstract void AddToStats(ref Dictionary<object, DataIndex> stats, T index);

    protected int[] AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new GeneFiltersCollection(criteria);

        var aggregation = AggregateFromGenes(property, criteria.Term, filters);

        return aggregation.Select(x => int.Parse(x.Key)).ToArrayOrNull();
    }

    protected int[] AggregateFromVariants<TProp>(Expression<Func<VariantIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new VariantFiltersCollection(criteria);

        var aggregation = AggregateFromVariants(property, criteria.Term, filters);

        return aggregation.Select(x => int.Parse(x.Key)).ToArrayOrNull();
    }


    private IDictionary<string, long> AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, string term, GeneFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Data);

        var result = _genesIndexService.Search(query).Result;

        return result.Aggregations[aggregationName];
    }

    private IDictionary<string, long> AggregateFromVariants<TProp>(Expression<Func<VariantIndex, TProp>> property, string term, VariantFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens)
            .AddExclusion(index => index.Data);

        var result = _variantsIndexService.Search(query).Result;

        return result.Aggregations[aggregationName];
    }
}
