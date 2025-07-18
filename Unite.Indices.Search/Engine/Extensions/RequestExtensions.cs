using System.Linq.Expressions;
using Nest;

namespace Unite.Indices.Search.Engine.Extensions;

public static class RequestExtensions
{
    /// <summary>
    /// Adds limits to given request if limits are set.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <param name="request">Sourse request</param>
    /// <param name="from">Start position</param>
    /// <param name="size">Number of rows</param>
    public static void AddLimits<T>(this ISearchRequest<T> request, int? from, int? size)
    {
        request.From = from;
        request.Size = size;
    }


    /// <summary>
    /// Includes filed to given request results.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Field type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Property to include</param>
    public static void Include<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property)
        where T : class
    {
        request.Source = new SourceFilterDescriptor<T>().Includes(x => x.Field(property));
    }

    /// <summary>
    /// Includes fields to given request results.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="properties">Properties to include</param>
    public static void Include<T>(this ISearchRequest<T> request,
        params Expression<Func<T, object>>[] properties)
        where T : class
    {
        request.Source = new SourceFilterDescriptor<T>().Includes(x => x.Fields(properties));
    }


    /// <summary>
    /// Excludes field from given request results.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Property to exclude</param>
    public static void Exclude<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property)
        where T : class
    {
        request.Source = new SourceFilterDescriptor<T>().Excludes(x => x.Field(property));
    }

    /// <summary>
    /// Excludes properties from given request results.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="properties">Properties to exclude</param>
    public static void Exclude<T>(this ISearchRequest<T> request,
        params Expression<Func<T, object>>[] properties)
        where T : class
    {
        request.Source = new SourceFilterDescriptor<T>().Excludes(x => x.Fields(properties));
    }


    /// <summary>
    /// Adds ordering to given request results.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Ordering property</param>
    /// <param name="order">Ordering type</param>
    public static void OrderBy<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property,
        SortOrder order = SortOrder.Ascending)
        where T : class
    {
        var sort = new FieldSortDescriptor<T>()
            .Field(property)
            .Order(order);

        if (request.Sort != null)
        {
            request.Sort.Add(sort);
        }
        else
        {
            request.Sort = [ sort ];
        }
    }
}
