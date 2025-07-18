using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class EqualityFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; set; }
    public bool IsEmpty => Values?.Any() != true;

    public Expression<Func<T, TProp>> Property { get; }
    public IEnumerable<TProp> Values { get; }


    public EqualityFilter(string name, Expression<Func<T, TProp>> property, IEnumerable<TProp> values)
    {
        Name = name;
        Not = false;

        Property = property;
        Values = values;
    }

    public EqualityFilter(string name, bool? not, Expression<Func<T, TProp>> property, IEnumerable<TProp> values)
    {
        Name = name;
        Not = not ?? false;

        Property = property;
        Values = values;
    }

    public EqualityFilter(string name, Expression<Func<T, TProp>> property, TProp value)
    {
        Name = name;
        Not = false;

        Property = property;
        Values = [value];
    }

    public EqualityFilter(string name, bool? not, Expression<Func<T, TProp>> property, TProp value)
    {
        Name = name;
        Not = not ?? false;

        Property = property;
        Values = [value];
    }


    public QueryContainer CreateQuery()
    {
        return !IsEmpty ? QueryExtensions.CreateTermsQuery(Property, Values) : null;
    }
}
