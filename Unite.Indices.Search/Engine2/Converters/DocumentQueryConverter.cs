using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine2.InputFilters;

namespace Unite.Indices.Search.Engine2.Converters;

/// <summary>
/// Converts input document queries to search engine filters.
/// </summary>
public static class DocumentQueryConverter
{
    private static readonly Dictionary<string, Type> _valueTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["string"] = typeof(string),
        ["int"] = typeof(int),
        ["long"] = typeof(long),
        ["double"] = typeof(double),
        ["bool"] = typeof(bool),
        ["date"] = typeof(DateTime)
    };

    private static readonly MethodInfo _createValueFilterMethod = typeof(DocumentQueryConverter)
        .GetMethod(nameof(CreateValueFilter), BindingFlags.NonPublic | BindingFlags.Static);

    private static readonly ConcurrentDictionary<(Type, Type), MethodInfo> _createValueFilterMethods = new();


    /// <summary>
    /// Returns the filter type name for the given value type (reverse of the filter type mapping).
    /// </summary>
    public static string GetFilterType(Type valueType)
    {
        var type = Nullable.GetUnderlyingType(valueType) ?? valueType;

        return _valueTypes.FirstOrDefault(entry => entry.Value == type).Key
            ?? throw new NotSupportedException($"Value type '{valueType.Name}' is not supported.");
    }

    public static CompoundFilter<TDocument> Convert<TDocument>(DocumentQuery<TDocument> query) where TDocument : class
    {
        return ConvertGroup<TDocument>(query?.FilterGroup ?? new FilterGroup());
    }

    private static IFilter<TDocument> ConvertNode<TDocument>(FilterNode node) where TDocument : class
    {
        return node switch
        {
            FilterGroup filterGroup => ConvertGroup<TDocument>(filterGroup),
            ValueFilter valueFilter => ConvertValueFilter<TDocument>(valueFilter),
            RangeFilter rangeFilter => ConvertRangeFilter<TDocument>(rangeFilter),
            ExistsFilter existsFilter => ConvertExistsFilter<TDocument>(existsFilter),
            SimilarityFilter similarityFilter => ConvertSimilarityFilter<TDocument>(similarityFilter),
            _ =>throw new NotSupportedException($"Filter node '{node?.GetType().Name}' is not supported.")
        };
    }

    private static CompoundFilter<TDocument> ConvertGroup<TDocument>(FilterGroup group) where TDocument : class
    {
        var filters = group.FilterNodes?
            .Select(ConvertNode<TDocument>)
            .ToList() ?? [];

        return new CompoundFilter<TDocument>(typeof(TDocument).Name, group.Negate, filters, group.Operator);
    }

    private static IFilter<TDocument> ConvertValueFilter<TDocument>(ValueFilter filter) where TDocument : class
    {
        if (!_valueTypes.TryGetValue(filter.FilterType ?? "", out var valueType))
            throw new NotSupportedException($"Filter type '{filter.FilterType}' is not supported.");

        var method = _createValueFilterMethods.GetOrAdd((typeof(TDocument), valueType),
            key => _createValueFilterMethod.MakeGenericMethod(key.Item1, key.Item2));

        return (IFilter<TDocument>)method.Invoke(null, BindingFlags.DoNotWrapExceptions, null, [filter], null);
    }

    private static IFilter<TDocument> CreateValueFilter<TDocument, TValue>(ValueFilter filter) where TDocument : class
    {
        var property = PropertyExpressionBuilder.Build<TDocument, TValue>(filter.Property);
        var converter = TypeDescriptor.GetConverter(typeof(TValue));
        var values = filter.Values?
            .Select(value => (TValue)converter.ConvertFromInvariantString(value))
            .ToArray();

        return new EqualityFilter<TDocument, TValue>(filter.Property, filter.Negate, property, values);
    }

    private static IFilter<TDocument> ConvertRangeFilter<TDocument>(RangeFilter filter) where TDocument : class
    {
        var property = PropertyExpressionBuilder.Build<TDocument, object>(filter.Property);

        return new Engine.Filters.RangeFilter<TDocument, object>(filter.Property, filter.Negate, property, filter.From, filter.To);
    }

    private static IFilter<TDocument> ConvertExistsFilter<TDocument>(ExistsFilter filter) where TDocument : class
    {
        var property = PropertyExpressionBuilder.Build<TDocument, object>(filter.Property);

        return new NotNullFilter<TDocument, object>(filter.Property, filter.Negate, property);
    }

    private static IFilter<TDocument> ConvertSimilarityFilter<TDocument>(SimilarityFilter filter) where TDocument : class
    {
        var property = PropertyExpressionBuilder.Build<TDocument, string>(filter.Property);

        return new Engine.Filters.SimilarityFilter<TDocument, string>(filter.Property, filter.Negate, property, filter.Values);
    }
}
