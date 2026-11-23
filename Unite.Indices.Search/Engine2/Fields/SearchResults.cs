using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Unite.Indices.Entities;

namespace Unite.Indices.Search.Engine2.Fields;

/// <summary>
/// How search results of documents of type <typeparamref name="TDocument"/> are returned:
/// sort orders (<see cref="SearchOrderAttribute"/>), properties left out of single documents (<see cref="ExcludedFromGetAttribute"/>)
/// and text fields of free-text searches (searchable text fields).
/// </summary>
public static class SearchResults<TDocument> where TDocument : class
{
    private static readonly Lazy<IReadOnlyList<(Expression<Func<TDocument, object>> Property, bool Descending)>> _orders = new(BuildOrders);
    private static readonly Lazy<IReadOnlyList<Expression<Func<TDocument, object>>>> _getExclusions = new(BuildGetExclusions);

    /// <summary>
    /// Sort orders by priority.
    /// </summary>
    public static IReadOnlyList<(Expression<Func<TDocument, object>> Property, bool Descending)> Orders => _orders.Value;

    /// <summary>
    /// Properties left out when a single document is requested.
    /// </summary>
    public static IReadOnlyList<Expression<Func<TDocument, object>>> GetExclusions => _getExclusions.Value;

    /// <summary>
    /// Searchable text fields, searched by free-text searches.
    /// </summary>
    public static IEnumerable<SearchField<TDocument, string>> TextFields => SearchFields<TDocument>.All.Values.OfType<SearchField<TDocument, string>>();


    private static IReadOnlyList<(Expression<Func<TDocument, object>>, bool)> BuildOrders()
    {
        return typeof(TDocument).GetCustomAttributes<SearchOrderAttribute>(false)
            .OrderBy(order => order.Priority)
            .Select(order => (ToObject(Resolve(order.Path)), order.Descending))
            .ToList();
    }

    private static IReadOnlyList<Expression<Func<TDocument, object>>> BuildGetExclusions()
    {
        var parameter = Expression.Parameter(typeof(TDocument), "document");

        return typeof(TDocument).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.IsDefined(typeof(ExcludedFromGetAttribute), true))
            .Select(property => ToObject(Expression.Lambda(Expression.Property(parameter, property), parameter)))
            .ToList();
    }

    /// <summary>
    /// Property of the dot-separated, camel-cased path (any public property, collections by their elements).
    /// </summary>
    /// <exception cref="InvalidOperationException">A property of the path does not exist.</exception>
    private static LambdaExpression Resolve(string path)
    {
        var parameter = Expression.Parameter(typeof(TDocument), "document");
        Expression access = parameter;

        foreach (var segment in path.Split('.'))
        {
            var property = access.Type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(property => JsonNamingPolicy.CamelCase.ConvertName(property.Name) == segment)
                ?? throw new InvalidOperationException($"Sort order '{path}' of {typeof(TDocument).Name}: '{segment}' does not exist on {access.Type.Name}.");

            access = Navigation.Access(access, property);
        }

        return Expression.Lambda(access, parameter);
    }

    private static Expression<Func<TDocument, object>> ToObject(LambdaExpression expression)
    {
        return Expression.Lambda<Func<TDocument, object>>(Expression.Convert(expression.Body, typeof(object)), expression.Parameters);
    }
}
