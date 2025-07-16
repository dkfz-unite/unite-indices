using Nest;

namespace Unite.Indices.Search.Engine.Filters;

public interface IFilter<T> where T : class
{
    string Name { get; }
    bool Not { get; set; }
    bool IsEmpty { get; }

    // void Apply(ISearchRequest<T> request);
    QueryContainer CreateQuery();
}
