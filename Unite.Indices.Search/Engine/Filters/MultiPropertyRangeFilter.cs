using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class MultiPropertyRangeFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }


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

        PropertyFrom = propertyFrom;
        PropertyTo = propertyTo;
        ValueFrom = valueFrom;
        ValueTo = valueTo;
    }


    public void Apply(ISearchRequest<T> request)
    {
        request.AddRangeQuery(PropertyFrom, PropertyTo, ValueFrom, ValueTo);
    }
}
