using Unite.Indices.Entities;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public interface ISearchService<T> where T : class
{
    Task<T> Get(string key);
    Task<SearchResult<T>> Search(SearchCriteria searchCriteria);
    Task<IReadOnlyDictionary<object, DataIndex>> Stats(SearchCriteria searchCriteria);
}
