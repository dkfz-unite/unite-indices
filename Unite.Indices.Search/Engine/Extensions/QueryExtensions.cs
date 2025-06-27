using System.Linq.Expressions;
using Nest;

namespace Unite.Indices.Search.Engine.Extensions;

public static class QueryExtensions
{
    /// <summary>
    /// Creates 'MultiMatch' query if filter value is set.
    /// </summary>
    /// <typeparam name="T">Index type'</typeparam>
    /// <param name="request">Source request'</param>
    /// <param name="value">Filter value'</param>
    /// <returns>QueryContainer with added 'MultiMatch' query.</returns>
    public static QueryContainer CreateMultiMatchQuery<T>(
        string value)
        where T : class
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(null, nameof(value));

        var query = Query<T>.MultiMatch(d => d
            .Query(value)
        );

        return query;
    }

    /// <summary>
    /// Creates 'Match' query if filter values are set.
    /// All separate filter values are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property">Filter property.</param>
    /// <param name="values">Filter values.</param>
    /// <returns>QueryContainer with added 'Match' query.</returns>
    public static QueryContainer CreateMatchQuery<T, TProp>(
        Expression<Func<T, TProp>> property,
        IEnumerable<string> values)
        where T : class
    {
        if (values == null || !values.Any())
            throw new ArgumentException(null, nameof(values));

        var query = values
            .Select(value =>
                Query<T>.Match(match => match
                    .Field(property)
                    .Query(value)
                    .Operator(Operator.And)
                )
            )
            .Aggregate((left, right) => left || right);

        return query;
    }

    /// <summary>
    /// Creates 'Match' query if filter values are set.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property1">First filter property.</param>
    /// <param name="property2">Second filter property.</param>
    /// <param name="values">Filter values.</param>
    /// <returns>QueryContainer with added 'Match' query.</returns>
    public static QueryContainer CreateMatchQuery<T, TProp>(
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        IEnumerable<string> values)
        where T : class
    {
        if (values == null || !values.Any())
            throw new ArgumentException(null, nameof(values));

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

        return query;
    }

    /// <summary>
    /// Creates 'Match' query if filter values are set.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property1">First filter property.</param>
    /// <param name="property2">Second filter property.</param>
    /// <param name="property3">Third filter property.</param>
    /// <param name="values">Filter values.</param>
    /// <returns>QueryContainer with added 'Match' query.</returns>
    public static QueryContainer CreateMatchQuery<T, TProp>(
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        Expression<Func<T, TProp>> property3,
        IEnumerable<string> values)
        where T : class
    {
        if (values == null || !values.Any())
            throw new ArgumentException(null, nameof(values));

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

        return query;
    }

    /// <summary>
    /// Creates 'Terms' query if filter values are set.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property">Filter property.</param>
    /// <param name="values">Filter values.</param>
    /// <returns>QueryContainer with added 'Terms' query.</returns>
    public static QueryContainer CreateTermsQuery<T, TProp>(
        Expression<Func<T, TProp>> property,
        IEnumerable<TProp> values)
        where T : class
    {
        if (values == null || !values.Any())
            throw new ArgumentException(null, nameof(values));

        var query = Query<T>.Terms(d => d
            .Field(property)
            .Terms(values)
        );

        return query;
    }

    /// <summary>
    /// Creates 'Terms' query if filter values are set.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property1">First filter property.</param>
    /// <param name="property2">Second filter property.</param>
    /// <param name="values">Filter values.</param>
    /// <returns>QueryContainer with added 'Terms' query.</returns>
    public static QueryContainer CreateTermsQuery<T, TProp>(
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        IEnumerable<TProp> values)
        where T : class
    {
        if (values == null || !values.Any())
            throw new ArgumentException(null, nameof(values));

        var query1 = Query<T>.Terms(d => d
            .Field(property1)
            .Terms(values)
        );

        var query2 = Query<T>.Terms(d => d
            .Field(property2)
            .Terms(values)
        );

        var query = query1 || query2;

        return query;
    }

    /// <summary>
    /// Creates 'Terms' query if filter values are set.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property1">First filter property.</param>
    /// <param name="property2">Second filter property.</param>
    /// <param name="property3">Third filter property.</param>
    /// <param name="values">Filter values.</param>
    /// <returns>QueryContainer with added 'Terms' query.</returns>
    public static QueryContainer CreateTermsQuery<T, TProp>(
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        Expression<Func<T, TProp>> property3,
        IEnumerable<TProp> values)
        where T : class
    {
        if (values == null || !values.Any())
            throw new ArgumentException(null, nameof(values));

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

        return query;
    }

    /// <summary>
    /// Creates boolean 'Terms' query if values is set.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property">Filter property.</param>
    /// <param name="value">Filter value.</param>
    /// <returns>QueryContainer with added 'Term' query.</returns>
    public static QueryContainer CreateBoolQuery<T>(
        Expression<Func<T, bool?>> property,
        bool? value)
        where T : class
    {
        if (value == null)
            throw new ArgumentException(null, nameof(value));

        var query = Query<T>.Term(term => term
            .Field(property)
            .Value(value)
        );

        return query;
    }

    /// <summary>
    /// Creates boolean 'Terms' query if values is set.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property1">First filter property.</param>
    /// <param name="property2">Second filter property.</param>
    /// <param name="property3">Third filter property.</param>
    /// <param name="value">Filter value.</param>
    /// <returns>QueryContainer with added 'Term' query.</returns>
    public static QueryContainer CreateBoolQuery<T>(
        Expression<Func<T, bool?>> property1,
        Expression<Func<T, bool?>> property2,
        Expression<Func<T, bool?>> property3,
        bool? value)
        where T : class
    {
        if (value == null)
            throw new ArgumentException(null, nameof(value));

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

        return query;
    }

    /// <summary>
    /// Creates 'Range' query if any of range bounds is set.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property">Filter property.</param>
    /// <param name="from">Filter range left bound value.</param>
    /// <param name="to">Filter range right bound value.</param>
    /// <returns>QueryContainer with added 'Range' query.</returns>
    public static QueryContainer CreateRangeQuery<T, TProp>(
        Expression<Func<T, TProp>> property,
        double? from,
        double? to)
        where T : class
    {
        if (from == null && to == null)
            throw new ArgumentException(null, nameof(from));

        var query = Query<T>.Range(d => d
            .Field(property)
            .GreaterThanOrEquals(from)
            .LessThanOrEquals(to)
        );

        return query;
    }

    /// <summary>
    /// Creates 'Range' query if any of range bounds is set.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="propertyFrom">Filter property from.</param>
    /// <param name="propertyTo">Filter property to.</param>
    /// <param name="from">Filter range left bound value.</param>
    /// <param name="to">Filter range right bound value.</param>
    /// <returns>QueryContainer with added 'Range' query.</returns>
    public static QueryContainer CreateRangeFilter<T, TProp>(
        Expression<Func<T, TProp>> propertyFrom,
        Expression<Func<T, TProp>> propertyTo,
        double? from,
        double? to)
        where T : class
    {
        if (from == null && to == null)
            throw new ArgumentException(null, nameof(from));

        var fromQuery = Query<T>.Range(d => d
            .Field(propertyFrom)
            .GreaterThanOrEquals(from)
        );

        var toQuery = Query<T>.Range(d => d
            .Field(propertyTo)
            .LessThanOrEquals(to)
        );

        var query = fromQuery && toQuery;

        return query;
    }

    /// <summary>
    /// Creates 'Range' query if any of range bounds is set.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property1">First filter property.</param>
    /// <param name="property2">Second filter property.</param>
    /// <param name="from">Filter range left bound value.</param>
    /// <param name="to">Filter range right bound value.</param>
    /// <returns>QueryContainer with added 'Range' query.</returns>
    public static QueryContainer CreateRangeQuery<T, TProp>(
        Expression<Func<T, TProp>> property1,
        Expression<Func<T, TProp>> property2,
        double? from,
        double? to)
        where T : class
    {
        if (from == null && to == null)
            throw new ArgumentException(null, nameof(from));

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

        return query;
    }

    /// <summary>
    /// Creates 'Range' query if any boundary value is set.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <param name="request">Request query.</param>
    /// <param name="value">Boundary value.</param>
    /// <param name="properties">Filter properties.</param>
    /// <typeparam name="T">Index Type.</typeparam>
    /// <typeparam name="TProp">Property Type.</typeparam>
    /// <returns>QueryContainer with added 'Range' query.</returns>
    public static QueryContainer CreateGreaterThanQuery<T, TProp>(
        double? value,
        params Expression<Func<T, TProp>>[] properties)
        where T : class
    {
        if (value == null)
            throw new ArgumentException(null, nameof(value));

        var query = properties
            .Select(property => Query<T>.Range(d => d
                .Field(property)
                .GreaterThanOrEquals(value)
            ))
            .Aggregate((left, right) => left || right);

        return query;
    }

    /// <summary>
    /// Creates 'Range' query if any boundary value is set.
    /// Property queries are combined with logical 'OR' operator.
    /// </summary>
    /// <param name="request">Request query.</param>
    /// <param name="value">Boundary value.</param>
    /// <param name="properties">Filter properties.</param>
    /// <typeparam name="T">Index Type.</typeparam>
    /// <typeparam name="TProp">Property Type.</typeparam>
    /// <returns>QueryContainer with added 'Range' query.</returns>
    public static QueryContainer CreateLessThanQuery<T, TProp>(
        double? value,
        params Expression<Func<T, TProp>>[] properties)
        where T : class
    {
        if (value == null)
            throw new ArgumentException(null, nameof(value));

        var query = properties
            .Select(property => Query<T>.Range(d => d
                .Field(property)
                .LessThanOrEquals(value)
            ))
            .Aggregate((left, right) => left || right);

        return query;
    }

    /// <summary>
    /// Creates 'Exists' query to check that given property is set.
    /// </summary>
    /// <typeparam name="T">Index type.</typeparam>
    /// <typeparam name="TProp">Property type.</typeparam>
    /// <param name="request">Request query.</param>
    /// <param name="property">Filter property.</param>
    /// <returns>QueryContainer with added 'Exists' query.</returns>
    public static QueryContainer CreateExistsQuery<T, TProp>(
        Expression<Func<T, TProp>> property)
        where T : class
    {
        var query = Query<T>.Exists(d => d
            .Field(property)
        );

        return query;
    }
}
