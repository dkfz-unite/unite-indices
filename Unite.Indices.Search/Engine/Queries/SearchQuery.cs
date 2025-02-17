using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;
using Unite.Indices.Search.Engine.Filters;

namespace Unite.Indices.Search.Engine.Queries;

public class SearchQuery<T> where T : class
{
    private SearchDescriptor<T> _request;
    private List<IFilter<T>> _filters;
    private List<Expression<Func<T, object>>> _exclusions;
    private List<string> _aggregations;

    public IEnumerable<IFilter<T>> Filters => _filters;
    public IEnumerable<Expression<Func<T, object>>> Exclusions => _exclusions;
    public IEnumerable<string> Aggregations => _aggregations;


    public SearchQuery()
    {
        _request = new SearchDescriptor<T>();
        _filters = new List<IFilter<T>>();
        _exclusions = new List<Expression<Func<T, object>>>();
        _aggregations = new List<string>();

        _request.TrackTotalHits();
    }


    public SearchQuery<T> AddPagination(int from, int size)
    {
        _request.AddLimits(from, size);

        return this;
    }

    public SearchQuery<T> AddFullTextSearch(string query)
    {
        _request.AddMultiMatchQuery(query);

        return this;
    }

    public SearchQuery<T> AddFilter(IFilter<T> filter)
    {
        _filters.Add(filter);

        return this;
    }

    public SearchQuery<T> AddFilters(IEnumerable<IFilter<T>> filters)
    {
        _filters.AddRange(filters);

        return this;
    }

    public SearchQuery<T> AddOrdering<TProp>(Expression<Func<T, TProp>> property, bool ascending = false)
    {
        _request.OrderBy(property, ascending ? SortOrder.Ascending : SortOrder.Descending);

        return this;
    }

    public SearchQuery<T> AddExclusion(Expression<Func<T, object>> property)
    {
        _exclusions.Add(property);

        return this;
    }

    public SearchQuery<T> AddAggregation<TProp>(string name, Expression<Func<T, TProp>> property)
    {
        if (!_aggregations.Contains(name))
        {
            // WARN!
            // Aggregation returns only top 1000 results.
            // This should be statistically sufficient for most nested search cases.
            _request.AddTermsAggregation(name, property, 1000);
            
            _aggregations.Add(name);
        }

        return this;
    }


    public ISearchRequest<T> GetRequest()
    {
        if (_filters.Any())
        {
            _filters.ForEach(filter => filter.Apply(_request));
        }

        if (_exclusions.Any())
        {
            _request.Exclude(_exclusions.ToArray());
        }

        return _request;
    }
}
