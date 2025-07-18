using System.Linq.Expressions;
using Nest;
using Unite.Indices.Search.Engine.Extensions;

namespace Unite.Indices.Search.Engine.Filters;

public class NotNullFilter<T, TProp> : IFilter<T> where T : class
{
    public string Name { get; }
    public bool Not { get; set; }
    public bool IsEmpty => false;

    public Expression<Func<T, TProp>> Property { get; }


    public NotNullFilter(string name, Expression<Func<T, TProp>> property)
    {
        Name = name;
        Not = false;

        Property = property;
    }

    public NotNullFilter(string name, bool? not, Expression<Func<T, TProp>> property)
    {
        Name = name;
        Not = not ?? false;

        Property = property;
    }


    public QueryContainer CreateQuery()
    {
        return QueryExtensions.CreateExistsQuery(Property);
    }
}
