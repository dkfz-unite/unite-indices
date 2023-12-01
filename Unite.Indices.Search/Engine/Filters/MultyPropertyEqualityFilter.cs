using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class MultyPropertyEqualityFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }

    public Expression<Func<T, TProp>> Property1 { get; }
    public Expression<Func<T, TProp>> Property2 { get; }
    public Expression<Func<T, TProp>> Property3 { get; }
    public IEnumerable<TProp> Values { get; }


    public MultyPropertyEqualityFilter(
        string name,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        Expression<Func<T, TProp>> property3,
        IEnumerable<TProp> values)
    {
        Name = name;

        Property1 = property1;
        Property2 = property2;
        Property3 = property3;
        Values = values;
    }

    public MultyPropertyEqualityFilter(
        string name,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        Expression<Func<T, TProp>> property3,
        TProp value)
    {
        Name = name;

        Property1 = property1;
        Property2 = property2;
        Property3 = property3;
        Values = new TProp[] { value };
    }


    public void Apply(ISearchRequest<T> request)
    {
        request.AddTermsQuery(Property1, Property2, Property3, Values);
    }
}
