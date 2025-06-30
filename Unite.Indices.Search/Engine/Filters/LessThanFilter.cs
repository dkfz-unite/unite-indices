using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class LessThanFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; }
    public bool IsEmpty => Value == null;

    public Expression<Func<T, TProp>>[] Properties { get; }
    public double? Value { get; }


    public LessThanFilter(string name, Expression<Func<T, TProp>>[] properties, double? value)
    {
        Name = name;
        Not = false;

        Properties = properties;
        Value = value;
    }

    public LessThanFilter(string name, bool? not, double? value, params Expression<Func<T, TProp>>[] properties)
    {
        Name = name;
        Not = not ?? false;

        Properties = properties;
        Value = value;
    }


    public QueryContainer CreateQuery()
    {
        return !IsEmpty ? QueryExtensions.CreateLessThanQuery(Value, Properties) : null;
    }
}
