using System.Numerics;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Criteria.Properties;
using LogicalOperator = Unite.Indices.Search.Engine2.Criteria.LogicalOperator;

namespace Unite.Indices.Search.Engine2.Converters;

/// <summary>
/// Converts input document criteria to search engine filters.
/// </summary>
public static class DocumentCriteriaConverter
{
    public static IFilter<TDocument> Convert<TDocument>(DocumentCriteria<TDocument> criteria) where TDocument : class
    {
        return ConvertNode<TDocument>(criteria ?? new CriteriaNode());
    }

    private static IFilter<TDocument> ConvertNode<TDocument>(CriteriaNode node) where TDocument : class
    {
        // Only property criteria have a condition of their own, other nodes only combine their child nodes.
        var ownFilter = (node as PropertyCriteria)?.Accept(FilterBuilder<TDocument>.Instance);

        if (node.Nodes == null || node.Nodes.Count == 0)
            return ownFilter ?? new CompoundFilter<TDocument>(typeof(TDocument).Name, node.Negate, [], Convert(node.Operator));

        var filters = new List<IFilter<TDocument>>();

        if (ownFilter != null)
        {
            // Negation applies to the whole node, not only to its own condition.
            ownFilter.Not = false;
            filters.Add(ownFilter);
        }

        filters.AddRange(node.Nodes.Select(ConvertNode<TDocument>));

        return new CompoundFilter<TDocument>(typeof(TDocument).Name, node.Negate, filters, Convert(node.Operator));
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


    /// <summary>
    /// Builds the filter for a property criteria of documents of type <typeparamref name="TDocument"/>.
    /// </summary>
    private sealed class FilterBuilder<TDocument> : IPropertyCriteriaVisitor<IFilter<TDocument>> where TDocument : class
    {
        public static readonly FilterBuilder<TDocument> Instance = new();

        public IFilter<TDocument> Visit<TValue>(ValueCriteria<TValue> criteria)
        {
            var property = PropertyExpressionBuilder.Build<TDocument, TValue>(criteria.Property);

            return new EqualityFilter<TDocument, TValue>(criteria.Property, criteria.Negate, property, criteria.Values);
        }

        public IFilter<TDocument> Visit<TValue>(RangeCriteria<TValue> criteria) where TValue : struct, INumber<TValue>
        {
            var property = PropertyExpressionBuilder.Build<TDocument, TValue>(criteria.Property);
            var from = criteria.From.HasValue ? double.CreateChecked(criteria.From.Value) : (double?)null;
            var to = criteria.To.HasValue ? double.CreateChecked(criteria.To.Value) : (double?)null;

            return new Engine.Filters.RangeFilter<TDocument, TValue>(criteria.Property, criteria.Negate, property, from, to);
        }

        public IFilter<TDocument> Visit(ExistsCriteria criteria)
        {
            var property = PropertyExpressionBuilder.Build<TDocument, object>(criteria.Property);

            return new NotNullFilter<TDocument, object>(criteria.Property, criteria.Negate, property);
        }

        public IFilter<TDocument> Visit(SimilarityCriteria criteria)
        {
            var property = PropertyExpressionBuilder.Build<TDocument, string>(criteria.Property);

            return new Engine.Filters.SimilarityFilter<TDocument, string>(criteria.Property, criteria.Negate, property, criteria.Values);
        }
    }
}
