using System.Linq.Expressions;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Criteria;
using Unite.Indices.Search.Services.Filters;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Indices.Search.Services;

public abstract class AggregatingSearchService
{
    public abstract IIndexService<GeneIndex> GenesIndexService { get; }
    public abstract IIndexService<VariantIndex> VariantsIndexService { get; }


    public int[] AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, SearchCriteria criteria)
    {
        IEnumerable<int> terms = null;

        if (criteria.Gene?.IsNotEmpty() == true)
        {
            var filters = new GeneIndexFiltersCollection(criteria);

            var termsFromGenes =  AggregateFromGenes(property, criteria.Term, filters);

            terms = terms == null ? termsFromGenes : terms.Intersect(termsFromGenes);
        }

        return terms?.ToArray();
    }

    public int[] AggregateFromVariants<TProp>(Expression<Func<VariantIndex, TProp>> property, SearchCriteria criteria)
    {
        IEnumerable<int> terms = null;

        if (criteria.Ssm?.IsNotEmpty() == true)
        {
            var filters = new SsmIndexFiltersCollection(criteria with { Cnv = null, Sv = null });

            var termsFromVariants = AggregateFromVariants(property, criteria.Term, filters);

            terms = terms == null ? termsFromVariants : terms.Intersect(termsFromVariants);
        }

        if (criteria.Cnv?.IsNotEmpty() == true)
        {
            var filters = new CnvIndexFiltersCollection(criteria with { Ssm = null, Sv = null });

            var termsFromVariants = AggregateFromVariants(property, criteria.Term, filters);

            terms = terms == null ? termsFromVariants : terms.Intersect(termsFromVariants);
        }

        if (criteria.Sv?.IsNotEmpty() == true)
        {
            var filters = new SvIndexFiltersCollection(criteria with { Ssm = null, Cnv = null });
            
            var termsFromVariants = AggregateFromVariants(property, criteria.Term, filters);

            terms = terms == null ? termsFromVariants : terms.Intersect(termsFromVariants);
        }

        return terms?.ToArray();
    }


    private int[] AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, string term, GeneIndexFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens);

        var result = GenesIndexService.Search(query).Result;

        var terms = result.Aggregations[aggregationName].Keys
            .Select(key => int.Parse(key))
            .ToArray();

        return terms;
    }

    private int[] AggregateFromVariants<TProp>(Expression<Func<VariantIndex, TProp>> property, string term, VariantFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens);

        var result = VariantsIndexService.Search(query).Result;

        var terms = result.Aggregations[aggregationName].Keys
            .Select(key => int.Parse(key))
            .ToArray();

        return terms;
    }
}
