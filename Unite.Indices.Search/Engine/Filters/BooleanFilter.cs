using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class BooleanFilter<T> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; set; }
    public bool IsEmpty => Value == null;

    public Expression<Func<T, bool?>> Property { get; }
    public bool? Value { get; }


    public BooleanFilter(string name, Expression<Func<T, bool?>> property, bool? value)
    {
        Name = name;
        Not = false;

        Property = property;
        Value = value;
    }

    public BooleanFilter(string name, bool? not, Expression<Func<T, bool?>> property, bool? value)
    {
        Name = name;
        Not = not ?? false;

        Property = property;
        Value = value;
    }


    public QueryContainer CreateQuery()
    {
        return !IsEmpty ? QueryExtensions.CreateBoolQuery(Property, Value) : null;
    }
}
