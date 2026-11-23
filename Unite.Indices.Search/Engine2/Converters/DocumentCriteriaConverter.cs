using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Criteria.Properties;
using Unite.Indices.Search.Engine2.Fields;
using LogicalOperator = Unite.Indices.Search.Engine2.Criteria.LogicalOperator;

namespace Unite.Indices.Search.Engine2.Converters;

/// <summary>
/// Converts input document criteria to search engine filters.
/// </summary>
public static class DocumentCriteriaConverter
{
    /// <exception cref="ArgumentException">A node has no kind, a criteria has no field, an unknown field or values not matching the field type.</exception>
    /// <summary>
    /// Converts a criteria tree of documents of type <typeparamref name="TDocument"/> (e.g. the include tree of their criteria).
    /// </summary>
    public static IFilter<TDocument> Convert<TDocument>(CriteriaGroup criteria) where TDocument : class
    {
        criteria ??= new CriteriaGroup();

        // Trees read from JSON are resolved already, trees built in code are resolved here.
        SearchFields<TDocument>.Resolve(criteria);

        return ConvertGroup<TDocument>(criteria);
    }

    private static IFilter<TDocument> ConvertNode<TDocument>(CriteriaNode node) where TDocument : class
    {
        return node switch
        {
            PropertyCriteria criteria => GetField<TDocument>(criteria).CreateFilter(criteria),
            CriteriaGroup group => ConvertGroup<TDocument>(group),
            _ => throw new InvalidOperationException($"Criteria node '{node?.GetType().Name}' is not supported.")
        };
    }

    private static IFilter<TDocument> ConvertGroup<TDocument>(CriteriaGroup group) where TDocument : class
    {
        var filters = group.Nodes?
            .Select(ConvertNode<TDocument>)
            .ToList() ?? [];

        return new CompoundFilter<TDocument>(typeof(TDocument).Name, group.Negate, filters, Convert(group.Operator));
    }

    private static SearchField<TDocument> GetField<TDocument>(PropertyCriteria criteria) where TDocument : class
    {
        return criteria.Property as SearchField<TDocument>
            ?? throw new InvalidOperationException($"Criteria property '{criteria.Property}' is not resolved for {typeof(TDocument).Name}.");
    }

    private static Engine.Filters.LogicalOperator Convert(LogicalOperator logicalOperator)
    {
        return logicalOperator switch
        {
            LogicalOperator.And => Engine.Filters.LogicalOperator.And,
            LogicalOperator.Or => Engine.Filters.LogicalOperator.Or,
            _ => throw new NotSupportedException($"LogicalOperator '{logicalOperator}' is not supported.")
        };
    }
}
