using System.Linq.Expressions;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class NotNullFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }

    public Expression<Func<T, TProp>> Property { get; }


    public NotNullFilter(string name, Expression<Func<T, TProp>> property)
    {
        Name = name;

        Property = property;
    }


    public void Apply(Nest.ISearchRequest<T> request)
    {
        request.AddExistsQuery(Property);
    }
}
