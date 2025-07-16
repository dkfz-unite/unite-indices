using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class MultiPropertyRangeFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; set; }
    public bool IsEmpty => ValueFrom == null && ValueTo == null;


    public Expression<Func<T, TProp>> PropertyFrom { get; }
    public Expression<Func<T, TProp>> PropertyTo { get; }
    public double? ValueFrom { get; }
    public double? ValueTo { get; }


    public MultiPropertyRangeFilter(
        string name,
        Expression<Func<T, TProp>> propertyFrom,
        Expression<Func<T, TProp>> propertyTo,
        double? valueFrom,
        double? valueTo)
    {
        Name = name;
        Not = false;

        PropertyFrom = propertyFrom;
        PropertyTo = propertyTo;
        ValueFrom = valueFrom;
        ValueTo = valueTo;
    }

    public MultiPropertyRangeFilter(
        string name,
        bool? not,
        Expression<Func<T, TProp>> propertyFrom,
        Expression<Func<T, TProp>> propertyTo,
        double? valueFrom,
        double? valueTo)
    {
        Name = name;
        Not = not ?? false;

        PropertyFrom = propertyFrom;
        PropertyTo = propertyTo;
        ValueFrom = valueFrom;
        ValueTo = valueTo;
    }
    

    public QueryContainer CreateQuery()
    {
        return !IsEmpty ? QueryExtensions.CreateRangeQuery(PropertyFrom, PropertyTo, ValueFrom, ValueTo) : null;
    }
}
