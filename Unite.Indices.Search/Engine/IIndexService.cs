using System.Linq.Expressions;
using Nest;
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
    Task<IEnumerable<IHit<T>>> Search(string query);

    /// <summary>
    /// Collects all distinct values of the given properties (by name) in documents matching the query.
    /// Pages through a composite aggregation per property; all properties share one request per page.
    /// </summary>
    Task<IReadOnlyDictionary<string, HashSet<string>>> CollectKeys(SearchQuery<T> query, IReadOnlyDictionary<string, LambdaExpression> properties);
}
