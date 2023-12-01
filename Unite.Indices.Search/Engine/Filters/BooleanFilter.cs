using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class BooleanFilter<T> : IFilter<T> where T : class
{
    public string Name { get; }

    public Expression<Func<T, bool?>> Property { get; }
    public bool? Value { get; }


    public BooleanFilter(string name, Expression<Func<T, bool?>> property, bool? value)
    {
        Name = name;

        Property = property;
        Value = value;
    }


    public void Apply(ISearchRequest<T> request)
    {
        request.AddBoolQuery(Property, Value);
    }
}
