using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class MultiPropertyEqualityFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; }
    public bool IsEmpty => Values?.Any() != true;

    public Expression<Func<T, TProp>> Property1 { get; }
    public Expression<Func<T, TProp>> Property2 { get; }
    public Expression<Func<T, TProp>> Property3 { get; }
    public IEnumerable<TProp> Values { get; }


    public MultiPropertyEqualityFilter(
        string name,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        Expression<Func<T, TProp>> property3,
        IEnumerable<TProp> values)
    {
        Name = name;
        Not = false;

        Property1 = property1;
        Property2 = property2;
        Property3 = property3;
        Values = values;
    }

    public MultiPropertyEqualityFilter(
        string name,
        bool? not,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        Expression<Func<T, TProp>> property3,
        IEnumerable<TProp> values)
    {
        Name = name;
        Not = not ?? false;

        Property1 = property1;
        Property2 = property2;
        Property3 = property3;
        Values = values;
    }

    public MultiPropertyEqualityFilter(
        string name,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        Expression<Func<T, TProp>> property3,
        TProp value)
    {
        Name = name;
        Not = false;

        Property1 = property1;
        Property2 = property2;
        Property3 = property3;
        Values = [value];
    }

    public MultiPropertyEqualityFilter(
            string name,
            bool? not,
            Expression<Func<T, TProp>> property1,
            Expression<Func<T, TProp>> property2,
            Expression<Func<T, TProp>> property3,
            TProp value)
    {
        Name = name;
        Not = not ?? false;

        Property1 = property1;
        Property2 = property2;
        Property3 = property3;
        Values = [value];
    }


    public QueryContainer CreateQuery()
    {
        return !IsEmpty ? QueryExtensions.CreateTermsQuery(Property1, Property2, Property3, Values) : null;
    }
}
