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
        _filters = [];
        _exclusions = [];
        _aggregations = [];

        _request.TrackTotalHits();
    }


    public SearchQuery<T> AddPagination(int from, int size)
    {
        _request.AddLimits(from, size);

        return this;
    }

    public SearchQuery<T> AddFullTextSearch(string query)
    {
        // TODO: Doesn't seem to work.
        _filters.Add(new MultiMatchFilter<T>("query", query));

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
        var query = new QueryContainer();

        var includeQueries = new List<QueryContainer>();
        var includeFilters = _filters.Where(filter => filter.Not == false);
        foreach (var filter in includeFilters)
        {
            if (!filter.IsEmpty)
                includeQueries.Add(filter.CreateQuery());
        }

        var excludeQueries = new List<QueryContainer>();
        var excludeFilters = _filters.Where(filter => filter.Not == true);
        foreach (var filter in excludeFilters)
        {
            if (!filter.IsEmpty)
                excludeQueries.Add(filter.CreateQuery());
        }

        if (includeQueries.Count != 0 && excludeQueries.Count != 0)
        {
            _request.Query(q => q.Bool(b => b
                .Must(includeQueries.ToArray())
                .MustNot(excludeQueries.ToArray())
            ));
        }
        else if (includeQueries.Count != 0)
        {
            _request.Query(q => q.Bool(b => b
                .Must(includeQueries.ToArray())
            ));
        }
        else if (excludeQueries.Count != 0)
        {
            _request.Query(q => q.Bool(b => b
                .MustNot(excludeQueries.ToArray())
            ));
        }

        return _request;
    }
}
