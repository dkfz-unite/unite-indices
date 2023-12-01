using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class EqualityFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }

    public Expression<Func<T, TProp>> Property { get; }
    public IEnumerable<TProp> Values { get; }


    public EqualityFilter(string name, Expression<Func<T, TProp>> property, IEnumerable<TProp> values)
    {
        Name = name;

        Property = property;
        Values = values;
    }

    public EqualityFilter(string name, Expression<Func<T, TProp>> property, TProp value)
    {
        Name = name;

        Property = property;
        Values = new TProp[] { value };
    }


    public void Apply(ISearchRequest<T> request)
    {
        request.AddTermsQuery(Property, Values);
    }
}
