using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Criteria;

namespace Unite.Indices.Search.Services;

public interface ISearchService<T>
    where T : class
{
    T Get(string key);
    SearchResult<T> Search(SearchCriteria searchCriteria = null);
}


public interface ISearchService<T, TContext>
    where T : class
    where TContext : class
{
    T Get(string key, TContext searchContext = null);
    SearchResult<T> Search(SearchCriteria searchCriteria = null, TContext searchContext = null);
}
