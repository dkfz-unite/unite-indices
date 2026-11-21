using System.Linq.Expressions;
using System.Reflection;

namespace Unite.Indices.Search.Engine2.Fields;

/// <summary>
/// Builds property access expressions through navigation properties.
/// Collections are accessed by their elements with First(), which NEST resolves to the field path of the element.
/// </summary>
internal static class Navigation
{
    private static readonly MethodInfo _firstMethod = typeof(Enumerable)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(method => method.Name == nameof(Enumerable.First) && method.GetParameters().Length == 1);


    /// <summary>
    /// Accesses the property; for collections the first element.
    /// </summary>
    public static Expression Access(Expression instance, PropertyInfo property)
    {
        Expression expression = Expression.Property(instance, property);
        var elementType = GetElementType(property.PropertyType);

        return elementType != null
            ? Expression.Call(_firstMethod.MakeGenericMethod(elementType), expression)
            : expression;
    }

    /// <summary>
    /// Element type of a collection type, null for other types (including string).
    /// </summary>
    public static Type GetElementType(Type type)
    {
        if (type == typeof(string))
            return null;

        if (type.IsArray)
            return type.GetElementType();

        return type.GetInterfaces()
            .Append(type)
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))?
            .GetGenericArguments()[0];
    }

    /// <summary>
    /// Simple value (number, text, date, ...), not a navigation object.
    /// </summary>
    public static bool IsValue(Type type)
    {
        var valueType = Nullable.GetUnderlyingType(type) ?? type;

        return valueType.IsPrimitive
            || valueType.IsEnum
            || valueType == typeof(string)
            || valueType == typeof(decimal)
            || valueType == typeof(DateTime)
            || valueType == typeof(DateTimeOffset)
            || valueType == typeof(Guid);
    }
}
