using Nest;

namespace Unite.Indices.Search.Engine.Filters;

public interface IFilter<T> where T : class
{
    string Name { get; }

    void Apply(ISearchRequest<T> request);
}
