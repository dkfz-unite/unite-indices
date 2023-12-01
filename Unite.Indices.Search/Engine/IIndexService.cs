using Unite.Indices.Search.Engine.Queries;

namespace Unite.Indices.Search.Engine;

/// <summary>
/// Index read service. Can get and search indices.
/// </summary>
public interface IIndexService<T>
    where T : class
{
    Task<T> Get(GetQuery<T> query);
    Task<SearchResult<T>> Search(SearchQuery<T> query);
}
