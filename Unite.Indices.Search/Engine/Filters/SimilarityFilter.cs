using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class SimilarityFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; set; }
    public bool IsEmpty => Values?.Any() != true;

    public Expression<Func<T, TProp>> Property { get; }
    public IEnumerable<string> Values { get; }


    public SimilarityFilter(string name, Expression<Func<T, TProp>> property, IEnumerable<string> values)
    {
        Name = name;
        Not = false;

        Property = property;
        Values = values;
    }

    public SimilarityFilter(string name, bool? not, Expression<Func<T, TProp>> property, IEnumerable<string> values)
    {
        Name = name;
        Not = not ?? false;

        Property = property;
        Values = values;
    }


    public QueryContainer CreateQuery()
    {
        return !IsEmpty ? QueryExtensions.CreateMatchQuery(Property, Values) : null;
    }
}
