using System.Numerics;
using Unite.Indices.Search.Engine2.Criteria.Properties;

namespace Unite.Indices.Search.Engine2.Fields;

/// <summary>
/// Checks that a criteria can be applied to a field. Returns an error message or null.
/// </summary>
internal sealed class CompatibilityCheck(SearchField field, Type fieldType) : IPropertyCriteriaVisitor<string>
{
    private readonly Type _valueType = Nullable.GetUnderlyingType(fieldType) ?? fieldType;

    public string Visit<TValue>(ValueCriteria<TValue> criteria)
    {
        return _valueType == typeof(TValue) ? null
            : $"'{field.Name}' has values of type {_valueType.Name}, criteria values are {typeof(TValue).Name}.";
    }

    public string Visit<TValue>(RangeCriteria<TValue> criteria) where TValue : struct, INumber<TValue>
    {
        return IsNumber(_valueType) ? null
            : $"'{field.Name}' has values of type {_valueType.Name}, range criteria need numbers.";
    }

    public string Visit(ExistsCriteria criteria)
    {
        return null;
    }

    public string Visit(SimilarityCriteria criteria)
    {
        return _valueType == typeof(string) ? null
            : $"'{field.Name}' has values of type {_valueType.Name}, similarity criteria need text.";
    }

    private static bool IsNumber(Type type)
    {
        return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INumber<>));
    }
}
