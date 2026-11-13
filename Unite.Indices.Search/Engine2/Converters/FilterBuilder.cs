using System.Numerics;
using Unite.Indices.Search.Engine.Filters;
using Unite.Indices.Search.Engine2.Criteria.Properties;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.Converters;

/// <summary>
/// Builds the search engine filter for a property criteria on the given field.
/// Criteria values (<c>TValue</c>) match the field type (<typeparamref name="TProp"/>) or its nullable form, checked on resolution.
/// </summary>
internal sealed class FilterBuilder<TDocument, TProp>(SearchField<TDocument, TProp> field) : IPropertyCriteriaVisitor<IFilter<TDocument>>
    where TDocument : class
{
    public IFilter<TDocument> Visit<TValue>(ValueCriteria<TValue> criteria)
    {
        var values = criteria.Values?.Select(value => (object)value).ToArray();

        return new EqualityFilter<TDocument, object>(field.Name, criteria.Negate, field.TermProperty, values);
    }

    public IFilter<TDocument> Visit<TValue>(RangeCriteria<TValue> criteria) where TValue : struct, INumber<TValue>
    {
        var from = criteria.From.HasValue ? double.CreateChecked(criteria.From.Value) : (double?)null;
        var to = criteria.To.HasValue ? double.CreateChecked(criteria.To.Value) : (double?)null;

        return new RangeFilter<TDocument, TProp>(field.Name, criteria.Negate, field.Property, from, to);
    }

    public IFilter<TDocument> Visit(ExistsCriteria criteria)
    {
        return new NotNullFilter<TDocument, TProp>(field.Name, criteria.Negate, field.Property);
    }

    public IFilter<TDocument> Visit(SimilarityCriteria criteria)
    {
        return new SimilarityFilter<TDocument, TProp>(field.Name, criteria.Negate, field.Property, criteria.Values);
    }
}
