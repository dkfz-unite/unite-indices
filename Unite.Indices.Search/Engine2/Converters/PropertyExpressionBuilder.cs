using System.Linq.Expressions;
using System.Reflection;

namespace Unite.Indices.Search.Engine2.Converters;

/// <summary>
/// Builds property expressions from dot-separated property paths (e.g. "Specimens.Id").
/// Collections are traversed with First(), which NEST resolves to the field path ("specimens.id").
/// </summary>
public static class PropertyExpressionBuilder
{
    private static readonly MethodInfo _firstMethod = typeof(Enumerable)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(method => method.Name == nameof(Enumerable.First) && method.GetParameters().Length == 1);

    public static Expression<Func<T, TProp>> Build<T, TProp>(string path)
    {
        var parameter = Expression.Parameter(typeof(T), "document");
        Expression body = parameter;

        foreach (var name in path.Split('.', StringSplitOptions.RemoveEmptyEntries))
        {
            var elementType = GetElementType(body.Type);

            if (elementType != null)
                body = Expression.Call(_firstMethod.MakeGenericMethod(elementType), body);

            var property = body.Type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                ?? throw new ArgumentException($"Property '{name}' not found on '{body.Type.Name}' (path '{path}').");

            body = Expression.Property(body, property);
        }

        if (body.Type != typeof(TProp))
            body = Expression.Convert(body, typeof(TProp));

        return Expression.Lambda<Func<T, TProp>>(body, parameter);
    }

    /// <summary>
    /// Reverse of <see cref="Build{T, TProp}"/>: returns the dot-separated property path of the expression
    /// (e.g. "Specimens.Id" for "image => image.Specimens.First().Id").
    /// </summary>
    public static string GetPath(LambdaExpression expression)
    {
        var names = new Stack<string>();
        var current = expression.Body;

        while (current is not ParameterExpression)
        {
            current = current switch
            {
                UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary => unary.Operand,
                MethodCallExpression { Method.Name: nameof(Enumerable.First), Arguments.Count: 1 } call => call.Arguments[0],
                MemberExpression member => Push(names, member.Member.Name, member.Expression),
                _ => throw new NotSupportedException($"Expression '{expression}' is not a property path.")
            };
        }

        return string.Join('.', names);
    }

    /// <summary>
    /// Returns the type of the property the expression points to, ignoring conversions (e.g. to object).
    /// </summary>
    public static Type GetPropertyType(LambdaExpression expression)
    {
        var body = expression.Body;

        while (body is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unary)
            body = unary.Operand;

        return body.Type;
    }

    private static Expression Push(Stack<string> names, string name, Expression next)
    {
        names.Push(name);
        return next;
    }

    private static Type GetElementType(Type type)
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
}
