using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class GreaterThanFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }

    public Expression<Func<T, TProp>>[] Properties { get; }
    public double? Value { get; }

    
    public GreaterThanFilter(string name, double? value, params Expression<Func<T, TProp>>[] properties)
    {
        Name = name;

        Properties = properties;
        Value = value;
    }


    public void Apply(ISearchRequest<T> request)
    {
        request.AddGreaterThanQuery(Value, Properties);
    }
}
