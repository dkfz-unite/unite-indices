using System.Numerics;

namespace Unite.Indices.Search.Engine2.Criteria.Properties;

/// <summary>
/// Visitor over property criteria: gives access to the value type of generic criteria without reflection.
/// </summary>
public interface IPropertyCriteriaVisitor<TResult>
{
    TResult Visit<TValue>(ValueCriteria<TValue> criteria);
    TResult Visit<TValue>(RangeCriteria<TValue> criteria) where TValue : struct, INumber<TValue>;
    TResult Visit(ExistsCriteria criteria);
    TResult Visit(SimilarityCriteria criteria);
}
