using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class RangeFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; set; }
    public bool IsEmpty => From == null && To == null;

    public Expression<Func<T, TProp>> Property { get; }
    public double? From { get; }
    public double? To { get; }


    public RangeFilter(string name, Expression<Func<T, TProp>> property, double? from, double? to)
    {
        Name = name;
        Not = false;

        Property = property;
        From = from;
        To = to;
    }

    public RangeFilter(string name, bool? not, Expression<Func<T, TProp>> property, double? from, double? to)
    {
        Name = name;
        Not = not ?? false;

        Property = property;
        From = from;
        To = to;
    }


    public QueryContainer CreateQuery()
    {
        return !IsEmpty ? QueryExtensions.CreateRangeQuery(Property, From, To) : null;
    }
}
