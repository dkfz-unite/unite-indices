using System.Linq.Expressions;
using Unite.Essentials.Extensions;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters;
using Unite.Indices.Search.Services.Filters.Criteria;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Indices.Search.Services;

public abstract class AggregatingSearchService
{
    public abstract IIndexService<GeneIndex> GenesIndexService { get; }
    public abstract IIndexService<VariantIndex> VariantsIndexService { get; }


    public int[] AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, SearchCriteria criteria)
    {
        var filters = new GeneFiltersCollection(criteria);

        var aggregation = AggregateFromGenes(property, criteria.Term, filters);

        return aggregation.Select(x => int.Parse(x.Key)).ToArrayOrNull();
    }

    public int[] AggregateFromVariants<TProp>(Expression<Func<VariantIndex, TProp>> property, SearchCriteria criteria)
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

        var result = GenesIndexService.Search(query).Result;

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

        var result = VariantsIndexService.Search(query).Result;

        return result.Aggregations[aggregationName];
    }
}
