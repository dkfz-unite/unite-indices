using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class RangeFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }

    public Expression<Func<T, TProp>> Property { get; }
    public double? From { get; }
    public double? To { get; }


    public RangeFilter(string name, Expression<Func<T, TProp>> property, double? from, double? to)
    {
        Name = name;

        Property = property;
        From = from;
        To = to;
    }


    public void Apply(ISearchRequest<T> request)
    {
        request.AddRangeQuery(Property, From, To);
    }
}
