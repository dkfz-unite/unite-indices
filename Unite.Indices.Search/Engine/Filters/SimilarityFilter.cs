using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class SimilarityFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }

    public Expression<Func<T, TProp>> Property { get; }
    public IEnumerable<string> Values { get; }


    public SimilarityFilter(string name, Expression<Func<T, TProp>> property, IEnumerable<string> values)
    {
        Name = name;

        Property = property;
        Values = values;
    }


    public void Apply(ISearchRequest<T> request)
    {
        request.AddMatchQuery(Property, Values);
    }
}
