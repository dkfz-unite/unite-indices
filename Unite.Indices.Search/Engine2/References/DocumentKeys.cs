using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Criteria.Properties;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.References;

/// <summary>
/// Key of an entity in a document: the document is the entity (identity) or refers to it.
/// </summary>
public abstract class DocumentKey(SearchEntity entity, bool isIdentity, bool isCollection, string name)
{
    public SearchEntity Entity { get; } = entity;

    /// <summary>
    /// The document is the entity itself, otherwise it refers to it.
    /// </summary>
    public bool IsIdentity { get; } = isIdentity;

    /// <summary>
    /// The document refers to the entity through a collection (many entities), otherwise through a single object.
    /// </summary>
    public bool IsCollection { get; } = isCollection;

    /// <summary>
    /// Path of the key in the document (e.g. "specimens.id").
    /// </summary>
    public string Name { get; } = name;
}

/// <summary>
/// Key of an entity in documents of type <typeparamref name="TDocument"/>.
/// </summary>
public abstract class DocumentKey<TDocument>(SearchEntity entity, bool isIdentity, bool isCollection, string name)
    : DocumentKey(entity, isIdentity, isCollection, name) where TDocument : class
{
    /// <summary>
    /// Criteria: the key is one of the given keys. More keys than <see cref="SearchLimits.MaxTermsPerQuery"/> are split into
    /// several criteria combined with OR, each within the limit of Elasticsearch terms queries.
    /// </summary>
    public abstract CriteriaNode CreateCriteria(IEnumerable<string> keys);

    /// <summary>
    /// The key property, e.g. "document => document.Specimens.First().Id".
    /// </summary>
    public abstract LambdaExpression KeyProperty { get; }
}

public sealed class DocumentKey<TDocument, TKey>(SearchEntity entity, bool isIdentity, bool isCollection, string name, Expression<Func<TDocument, TKey>> key)
    : DocumentKey<TDocument>(entity, isIdentity, isCollection, name) where TDocument : class
{
    private static readonly TypeConverter _keyConverter = TypeDescriptor.GetConverter(typeof(TKey));

    private readonly SearchField<TDocument, TKey> _field = new(name, key);

    public Expression<Func<TDocument, TKey>> Key { get; } = key;


    public override CriteriaNode CreateCriteria(IEnumerable<string> keys)
    {
        return SearchLimits.CreateValueCriteria(_field, keys.Select(value => (TKey)_keyConverter.ConvertFromInvariantString(value)));
    }

    public override LambdaExpression KeyProperty => Key;
}

/// <summary>
/// Entity keys of documents of type <typeparamref name="TDocument"/>, by entity type.
/// Built from <see cref="EntityKeyAttribute"/> (identity) and <see cref="ReferencesAttribute"/> (references, also nested below
/// navigation properties marked with <see cref="SearchableAttribute"/>).
/// </summary>
public static class DocumentKeys<TDocument> where TDocument : class
{
    private static readonly Lazy<IReadOnlyDictionary<Type, DocumentKey<TDocument>>> _keys = new(Build);

    public static IReadOnlyDictionary<Type, DocumentKey<TDocument>> All => _keys.Value;


    private static IReadOnlyDictionary<Type, DocumentKey<TDocument>> Build()
    {
        var keys = new Dictionary<Type, DocumentKey<TDocument>>();
        var parameter = Expression.Parameter(typeof(TDocument), "document");

        if (SearchEntities.All.TryGetValue(typeof(TDocument), out var identity))
        {
            var access = Expression.Property(parameter, identity.Key);

            keys.Add(identity.Document.Type, CreateKey(identity, true, false, GetName(identity.Key), access, parameter));
        }

        Collect(keys, parameter, parameter, typeof(TDocument), null, false, [typeof(TDocument)]);

        return keys;
    }

    /// <summary>
    /// Collects references of the type: [References] properties, also below navigation properties marked with [Searchable]
    /// (e.g. "affectedFeatures[ ].gene.id" of variants). Referenced entities are not descended into.
    /// </summary>
    private static void Collect(Dictionary<Type, DocumentKey<TDocument>> keys, ParameterExpression parameter, Expression access,
        Type type, string prefix, bool inCollection, HashSet<Type> path)
    {
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(property => property.GetIndexParameters().Length == 0))
        {
            var navigation = Navigation.Access(access, property);
            var name = prefix == null ? GetName(property) : $"{prefix}.{GetName(property)}";
            var isCollection = inCollection || Navigation.GetElementType(property.PropertyType) != null;
            var attribute = property.GetCustomAttribute<ReferencesAttribute>(true);

            if (attribute != null)
            {
                var location = $"{typeof(TDocument).Name}.{name}";

                if (!SearchEntities.All.TryGetValue(attribute.Entity, out var entity))
                    throw new InvalidOperationException($"{location} refers to {attribute.Entity.FullName}, which is not a searchable entity.");

                var key = SearchEntities.FindKey(navigation.Type);

                if (key == null || key.PropertyType != entity.Key.PropertyType)
                    throw new InvalidOperationException($"{location} refers to {entity.Document.Type.Name}, but {navigation.Type.Name} has no entity key property of type {entity.Key.PropertyType.Name}.");

                if (!keys.TryAdd(entity.Document.Type, CreateKey(entity, false, isCollection, $"{name}.{GetName(key)}", Expression.Property(navigation, key), parameter)))
                    throw new InvalidOperationException($"{typeof(TDocument).Name} refers to {entity.Document.Type.Name} more than once ({name}).");
            }
            else if (property.IsDefined(typeof(SearchableAttribute), true) && !Navigation.IsValue(navigation.Type) && path.Add(navigation.Type))
            {
                Collect(keys, parameter, navigation, navigation.Type, name, isCollection, path);
                path.Remove(navigation.Type);
            }
        }
    }

    private static DocumentKey<TDocument> CreateKey(SearchEntity entity, bool isIdentity, bool isCollection, string name, Expression access, ParameterExpression parameter)
    {
        var key = Expression.Lambda(access, parameter);
        var keyType = typeof(DocumentKey<,>).MakeGenericType(typeof(TDocument), access.Type);

        return (DocumentKey<TDocument>)Activator.CreateInstance(keyType, entity, isIdentity, isCollection, name, key);
    }

    private static string GetName(PropertyInfo property)
    {
        return JsonNamingPolicy.CamelCase.ConvertName(property.Name);
    }
}

/// <summary>
/// Limits of Elasticsearch queries built by the search.
/// </summary>
public static class SearchLimits
{
    /// <summary>
    /// Maximum number of values of one terms query (Elasticsearch index setting "index.max_terms_count", default 65,536).
    /// </summary>
    public static int MaxTermsPerQuery { get; set; } = 65_536;

    /// <summary>
    /// Criteria: the field has one of the values. More values than <see cref="MaxTermsPerQuery"/> are split into
    /// several criteria combined with OR; negating the result still negates all values.
    /// </summary>
    public static CriteriaNode CreateValueCriteria<TValue>(SearchField field, IEnumerable<TValue> values)
    {
        var list = values.ToList();

        if (list.Count <= MaxTermsPerQuery)
            return new ValueCriteria<TValue> { Property = field, Values = list };

        return new CriteriaGroup
        {
            Operator = LogicalOperator.Or,
            Nodes = list.Chunk(MaxTermsPerQuery)
                .Select(chunk => (CriteriaNode)new ValueCriteria<TValue> { Property = field, Values = [.. chunk] })
                .ToList()
        };
    }
}
