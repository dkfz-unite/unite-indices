using Unite.Indices.Entities;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Indices.Search.Services;

public interface ISearchService<T> where T : class
{
    T Get(string key);
    SearchResult<T> Search(SearchCriteria searchCriteria);
    IReadOnlyDictionary<object, DataIndex> Stats(SearchCriteria searchCriteria);
}
