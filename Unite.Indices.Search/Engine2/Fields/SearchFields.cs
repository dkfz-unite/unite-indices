using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Criteria.Properties;

namespace Unite.Indices.Search.Engine2.Fields;

/// <summary>
/// Searchable fields of documents of type <typeparamref name="TDocument"/>, built from <see cref="SearchableAttribute"/> markup.
/// A nested property is searchable only if every property on the path to it is marked.
/// </summary>
public static class SearchFields<TDocument> where TDocument : class
{
    private static readonly MethodInfo _firstMethod = typeof(Enumerable)
        .GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Single(method => method.Name == nameof(Enumerable.First) && method.GetParameters().Length == 1);

    private static readonly Lazy<IReadOnlyDictionary<string, SearchField<TDocument>>> _fields = new(Build);

    public static IReadOnlyDictionary<string, SearchField<TDocument>> All => _fields.Value;


    public static SearchField<TDocument> Find(string name)
    {
        return name != null && All.TryGetValue(name, out var field) ? field : null;
    }

    /// <summary>
    /// Replaces field names in all property criteria of the tree with typed fields of the document.
    /// </summary>
    /// <exception cref="ArgumentException">A node has no kind, a criteria has no field, an unknown field or values not matching the field type.</exception>
    public static void Resolve(CriteriaGroup root)
    {
        foreach (var node in Descendants(root))
        {
            if (node is not PropertyCriteria criteria)
                continue;

            if (criteria.Property is SearchField<TDocument>)
                continue;

            if (criteria.Property == null)
                throw new ArgumentException("Criteria property is required.");

            var field = Find(criteria.Property.Name)
                ?? throw new ArgumentException($"'{criteria.Property.Name}' is not a searchable property of {typeof(TDocument).Name}.");

            var error = criteria.Accept(new CompatibilityCheck(field, field.ValueType));

            if (error != null)
                throw new ArgumentException(error);

            criteria.Property = field;
        }
    }


    /// <summary>
    /// All nodes below the group. Groups are descended into, plain nodes (without kind) and missing nodes are rejected.
    /// </summary>
    private static IEnumerable<CriteriaNode> Descendants(CriteriaGroup group)
    {
        foreach (var node in group.Nodes ?? [])
        {
            if (node == null)
                throw new ArgumentException("Criteria node is required.");

            if (node.GetType() == typeof(CriteriaNode))
                throw new ArgumentException("Criteria node kind is required.");

            yield return node;

            if (node is CriteriaGroup child)
            {
                foreach (var descendant in Descendants(child))
                    yield return descendant;
            }
        }
    }

    private static IReadOnlyDictionary<string, SearchField<TDocument>> Build()
    {
        var fields = new Dictionary<string, SearchField<TDocument>>(StringComparer.OrdinalIgnoreCase);
        var parameter = Expression.Parameter(typeof(TDocument), "document");

        Collect(fields, parameter, parameter, typeof(TDocument), null, [typeof(TDocument)]);

        return fields;
    }

    private static void Collect(
        Dictionary<string, SearchField<TDocument>> fields,
        ParameterExpression parameter,
        Expression access,
        Type type,
        string prefix,
        HashSet<Type> path)
    {
        foreach (var property in GetSearchableProperties(type))
        {
            var attribute = property.GetCustomAttribute<SearchableAttribute>(true);
            var segment = attribute.Name ?? JsonNamingPolicy.CamelCase.ConvertName(property.Name);
            var name = prefix == null ? segment : $"{prefix}.{segment}";

            Expression expression = Expression.Property(access, property);
            var valueType = property.PropertyType;
            var elementType = GetElementType(valueType);

            // Collections are searched by their elements: NEST resolves First() to the field path of the element.
            if (elementType != null)
            {
                expression = Expression.Call(_firstMethod.MakeGenericMethod(elementType), expression);
                valueType = elementType;
            }

            if (IsValue(valueType))
            {
                var field = CreateField(name, Expression.Lambda(expression, parameter));

                if (!fields.TryAdd(name, field))
                    throw new InvalidOperationException($"Searchable property '{name}' of {typeof(TDocument).Name} is declared twice.");
            }
            else if (path.Add(valueType))
            {
                Collect(fields, parameter, expression, valueType, name, path);
                path.Remove(valueType);
            }
            else
            {
                throw new InvalidOperationException($"Searchable property '{name}' of {typeof(TDocument).Name} creates a cycle through {valueType.Name}.");
            }
        }
    }

    private static SearchField<TDocument> CreateField(string name, LambdaExpression property)
    {
        var fieldType = typeof(SearchField<,>).MakeGenericType(typeof(TDocument), property.ReturnType);

        return (SearchField<TDocument>)Activator.CreateInstance(fieldType, name, property);
    }

    /// <summary>
    /// Marked public instance properties; for properties hidden with 'new' only the most derived one.
    /// </summary>
    private static IEnumerable<PropertyInfo> GetSearchableProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .GroupBy(property => property.Name)
            .Select(group => group.OrderByDescending(property => GetDepth(property.DeclaringType)).First())
            .Where(property => property.IsDefined(typeof(SearchableAttribute), true));
    }

    private static int GetDepth(Type type)
    {
        var depth = 0;

        for (var current = type; current != null; current = current.BaseType)
            depth++;

        return depth;
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

    private static bool IsValue(Type type)
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
