﻿using System.Linq.Expressions;
using Nest;

namespace Unite.Indices.Search.Engine.Extensions;

public static class QueryExtensions
{
    /// <summary>
    /// Adds 'MultyMatch' query to given request if filter value is set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="value">Filter value</param>
    public static void AddMultiMatchQuery<T>(this ISearchRequest<T> request,
        string value)
        where T : class
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var query = Query<T>.MultiMatch(d => d
            .Query(value)
        );

        request.Query = SetOrAdd(request.Query, query);
    }


    /// <summary>
    /// Adds 'Match' query to given request if filter values are set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// All separate filter values are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Filter property</param>
    /// <param name="values">Filter values</param>
    public static void AddMatchQuery<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property,
        IEnumerable<string> values)
        where T : class
    {
        if (values == null || !values.Any())
        {
            return;
        }

        var query = values
            .Select(value =>
                Query<T>.Match(match => match
                    .Field(property)
                    .Query(value)
                    .Operator(Operator.And)
                )
            )
            .Aggregate((left, right) => left || right);

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds 'Match' query to given request if filter values are set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property1">First filter property</param>
    /// <param name="property2">Second filter property</param>
    /// <param name="values">Filter values</param>
    public static void AddMatchQuery<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        IEnumerable<string> values)
        where T : class
    {
        if (values == null || !values.Any())
        {
            return;
        }

        var query1 = values
            .Select(value =>
                Query<T>.Match(match => match
                    .Field(property1)
                    .Query(value)
                    .Operator(Operator.And)
                )
            )
            .Aggregate((left, right) => left || right);

        var query2 = values
            .Select(value =>
                Query<T>.Match(match => match
                    .Field(property2)
                    .Query(value)
                    .Operator(Operator.And)
                )
            )
            .Aggregate((left, right) => left || right);

        var query = query1 || query2;

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds 'Match' query to given request if filter values are set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property1">First filter property</param>
    /// <param name="property2">Second filter property</param>
    /// <param name="property3">Third filter property</param>
    /// <param name="values">Filter values</param>
    public static void AddMatchQuery<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        Expression<Func<T, TProp>> property3,
        IEnumerable<string> values)
        where T : class
    {
        if (values == null || !values.Any())
        {
            return;
        }

        var query1 = values
            .Select(value =>
                Query<T>.Match(match => match
                    .Field(property1)
                    .Query(value)
                    .Operator(Operator.And)
                )
            )
            .Aggregate((left, right) => left || right);

        var query2 = values
            .Select(value =>
                Query<T>.Match(match => match
                    .Field(property2)
                    .Query(value)
                    .Operator(Operator.And)
                )
            )
            .Aggregate((left, right) => left || right);

        var query3 = values
            .Select(value =>
                Query<T>.Match(match => match
                    .Field(property3)
                    .Query(value)
                    .Operator(Operator.And)
                )
            )
            .Aggregate((left, right) => left || right);

        var query = query1 || query2 || query3;

        request.Query = SetOrAdd(request.Query, query);
    }


    /// <summary>
    /// Adds 'Terms' query to given request if filter values are set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Filter property</param>
    /// <param name="values">Filter values</param>
    public static void AddTermsQuery<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property,
        IEnumerable<TProp> values)
        where T : class
    {
        if (values == null || !values.Any())
        {
            return;
        }

        var query = Query<T>.Terms(d => d
            .Field(property)
            .Terms(values)
        );

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds 'Terms' query to given request if filter values are set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property1">First filter property</param>
    /// <param name="property2">Second filter property</param>
    /// <param name="values">Filter values</param>
    public static void AddTermsQuery<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        IEnumerable<TProp> values)
        where T : class
    {
        if (values == null || !values.Any())
        {
            return;
        }

        var query1 = Query<T>.Terms(d => d
            .Field(property1)
            .Terms(values)
        );

        var query2 = Query<T>.Terms(d => d
            .Field(property2)
            .Terms(values)
        );

        var query = query1 || query2;

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds 'Terms' query to given request if filter values are set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property1">First filter property</param>
    /// <param name="property2">Second filter property</param>
    /// <param name="property3">Third filter property</param>
    /// <param name="values">Filter values</param>
    public static void AddTermsQuery<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        Expression<Func<T, TProp>> property3,
        IEnumerable<TProp> values)
        where T : class
    {
        if (values == null || !values.Any())
        {
            return;
        }

        var query1 = Query<T>.Terms(d => d
            .Field(property1)
            .Terms(values)
        );

        var query2 = Query<T>.Terms(d => d
            .Field(property2)
            .Terms(values)
        );

        var query3 = Query<T>.Terms(d => d
            .Field(property3)
            .Terms(values)
        );

        var query = query1 || query2 || query3;

        request.Query = SetOrAdd(request.Query, query);
    }


    /// <summary>
    /// Adds boolean 'Terms' query to given request if values is set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Filter property</param>
    /// <param name="value">Filter value</param>
    public static void AddBoolQuery<T>(this ISearchRequest<T> request,
        Expression<Func<T, bool?>> property,
        bool? value)
        where T : class
    {
        if (!value.HasValue)
        {
            return;
        }

        var query = Query<T>.Term(term => term
            .Field(property)
            .Value(value)
        );

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds boolean 'Terms' query to given request if values is set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property1">First filter property</param>
    /// <param name="property2">Second filter property</param>
    /// <param name="property3">Third filter property</param>
    /// <param name="value">Filter value</param>
    public static void AddBoolQuery<T>(this ISearchRequest<T> request,
        Expression<Func<T, bool?>> property1,
        Expression<Func<T, bool?>> property2,
        Expression<Func<T, bool?>> property3,
        bool? value)
        where T : class
    {
        if (!value.HasValue)
        {
            return;
        }

        var query1 = Query<T>.Term(term => term
            .Field(property1)
            .Value(value)
        );

        var query2 = Query<T>.Term(term => term
            .Field(property2)
            .Value(value)
        );

        var query3 = Query<T>.Term(term => term
            .Field(property3)
            .Value(value)
        );

        var query = query1 || query2 || query3;

        request.Query = SetOrAdd(request.Query, query);
    }


    /// <summary>
    /// Adds 'Range' query to given request if any of range bounds is set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Filter property</param>
    /// <param name="from">Filter range left bound value</param>
    /// <param name="to">Filter range right bound value</param>
    public static void AddRangeQuery<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property,
        double? from,
        double? to)
        where T : class
    {
        if (!from.HasValue && !to.HasValue)
        {
            return;
        }

        var query = Query<T>.Range(d => d
            .Field(property)
            .GreaterThanOrEquals(from)
            .LessThanOrEquals(to)
        );

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds 'Range' query to given request if any or gange bounds is set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="propertyFrom">Filter property from</param>
    /// <param name="propertyTo">Filter property to</param>
    /// <param name="from">Filter range left bound value</param>
    /// <param name="to">Filter range right bound value</param>
    public static void AddRangeFilter<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> propertyFrom,
        Expression<Func<T, TProp>> propertyTo,
        double? from,
        double? to)
        where T : class
    {
        if (!from.HasValue && !to.HasValue)
        {
            return;
        }

        var fromQuery = Query<T>.Range(d => d
            .Field(propertyFrom)
            .GreaterThanOrEquals(from)
        );

        var toQuery = Query<T>.Range(d => d
            .Field(propertyTo)
            .LessThanOrEquals(to)
        );

        var query = fromQuery && toQuery;

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds 'Range' query to given request if any of range bounds is set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Filter property</param>
    /// <param name="from">Filter range left bound value</param>
    /// <param name="to">Filter range right bound value</param>
    public static void AddRangeQuery<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        double? from,
        double? to)
        where T : class
    {
        if (!from.HasValue && !to.HasValue)
        {
            return;
        }

        var query1 = Query<T>.Range(d => d
            .Field(property1)
            .GreaterThanOrEquals(from)
            .LessThanOrEquals(to)
        );

        var query2 = Query<T>.Range(d => d
            .Field(property2)
            .GreaterThanOrEquals(from)
            .LessThanOrEquals(to)
        );

        var query = query1 || query2;

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds 'Range' query to given request if any boundary value is set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <param name="request">Source request.</param>
    /// <param name="value">Boundary value.</param>
    /// <param name="properties">Filter properties.</param>
    /// <typeparam name="T">Index Type.</typeparam>
    /// <typeparam name="TProp">Property Type.</typeparam>
    public static void AddGreaterThanQuery<T, TProp>(this ISearchRequest<T> request,
        double? value,
        params Expression<Func<T, TProp>>[] properties)
        where T : class
    {
        if (!value.HasValue)
        {
            return;
        }

        var query = properties
            .Select(property => Query<T>.Range(d => d
                .Field(property)
                .GreaterThanOrEquals(value)
            ))
            .Aggregate((left, right) => left || right);

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds 'Range' query to given request if any boundary value is set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <param name="request">Source request.</param>
    /// <param name="value">Boundary value.</param>
    /// <param name="properties">Filter properties.</param>
    /// <typeparam name="T">Index Type.</typeparam>
    /// <typeparam name="TProp">Property Type.</typeparam>
    public static void AddLessThanQuery<T, TProp>(this ISearchRequest<T> request,
        double? value,
        params Expression<Func<T, TProp>>[] properties)
        where T : class
    {
        if (!value.HasValue)
        {
            return;
        }

        var query = properties
            .Select(property => Query<T>.Range(d => d
                .Field(property)
                .LessThanOrEquals(value)
            ))
            .Aggregate((left, right) => left || right);

        request.Query = SetOrAdd(request.Query, query);
    }

    /// <summary>
    /// Adds 'Exists' query to check that given property is set.
    /// Creates new query or adds query to existing request query with logical 'AND' operator.
    /// </summary>
    /// <typeparam name="T">Index type</typeparam>
    /// <typeparam name="TProp">Property type</typeparam>
    /// <param name="request">Source request</param>
    /// <param name="property">Filter property</param>
    public static void AddExistsQuery<T, TProp>(this ISearchRequest<T> request,
        Expression<Func<T, TProp>> property)
        where T : class
    {
        var query = Query<T>.Exists(d => d
            .Field(property)
        );

        request.Query = SetOrAdd(request.Query, query);
    }


    private static QueryContainer SetOrAdd(QueryContainer sourceQuery, QueryContainer newQuery)
    {
        if (sourceQuery == null)
        {
            return newQuery;
        }
        else
        {
            return sourceQuery && newQuery;
        }
    }
}
