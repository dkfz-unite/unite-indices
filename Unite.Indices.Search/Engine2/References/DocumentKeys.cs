using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Engine2.Converters;
using Unite.Indices.Search.Engine2.Criteria;
using Unite.Indices.Search.Engine2.Criteria.Properties;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.References;

/// <summary>
/// Key of an entity in a document: the document is the entity (identity) or refers to it.
/// </summary>
public abstract class DocumentKey(SearchEntity entity, bool isIdentity, string name)
{
    public SearchEntity Entity { get; } = entity;

    /// <summary>
    /// The document is the entity itself, otherwise it refers to it.
    /// </summary>
    public bool IsIdentity { get; } = isIdentity;

    /// <summary>
    /// Path of the key in the document (e.g. "specimens.id").
    /// </summary>
    public string Name { get; } = name;
}

/// <summary>
/// Key of an entity in documents of type <typeparamref name="TDocument"/>.
/// </summary>
public abstract class DocumentKey<TDocument>(SearchEntity entity, bool isIdentity, string name) : DocumentKey(entity, isIdentity, name)
    where TDocument : class
{
    /// <summary>
    /// Criteria: the key is one of the given keys.
    /// </summary>
    public abstract PropertyCriteria CreateCriteria(IEnumerable<string> keys);

    /// <summary>
    /// Keys of all documents matching the criteria.
    /// </summary>
    public abstract Task<HashSet<string>> FindKeys(DocumentCriteria<TDocument> criteria, IServiceProvider serviceProvider);
}

public sealed class DocumentKey<TDocument, TKey>(SearchEntity entity, bool isIdentity, string name, Expression<Func<TDocument, TKey>> key)
    : DocumentKey<TDocument>(entity, isIdentity, name) where TDocument : class
{
    // Not "keys": NEST reserves it for parsing aggregation responses.
    private const string AggregationName = "entity_keys";
    private static readonly TypeConverter _keyConverter = TypeDescriptor.GetConverter(typeof(TKey));

    private readonly SearchField<TDocument, TKey> _field = new(name, key);

    public Expression<Func<TDocument, TKey>> Key { get; } = key;


    public override PropertyCriteria CreateCriteria(IEnumerable<string> keys)
    {
        return new ValueCriteria<TKey>
        {
            Property = _field,
            Values = keys.Select(value => (TKey)_keyConverter.ConvertFromInvariantString(value)).ToList()
        };
    }

    public override async Task<HashSet<string>> FindKeys(DocumentCriteria<TDocument> criteria, IServiceProvider serviceProvider)
    {
        // WARN: the terms aggregation returns at most 1000 keys (see SearchQuery.AddAggregation).
        var query = new SearchQuery<TDocument>()
            .AddPagination(0, 0)
            .AddFilter(DocumentCriteriaConverter.Convert(criteria))
            .AddAggregation(AggregationName, Key);

        var result = await serviceProvider.GetRequiredService<IIndexService<TDocument>>().Search(query);

        return result.Aggregations.TryGetValue(AggregationName, out var buckets) && buckets != null
            ? buckets.Keys.ToHashSet()
            : [];
    }
}

/// <summary>
/// Entity keys of documents of type <typeparamref name="TDocument"/>, by entity type.
/// Built from <see cref="SearchableDocumentAttribute.Key"/> (identity) and <see cref="ReferencesAttribute"/> (references).
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

            keys.Add(identity.Document.Type, CreateKey(identity, true, GetName(identity.Key), access, parameter));
        }

        foreach (var property in typeof(TDocument).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var attribute = property.GetCustomAttribute<ReferencesAttribute>(true);

            if (attribute == null)
                continue;

            var name = $"{typeof(TDocument).Name}.{property.Name}";

            if (!SearchEntities.All.TryGetValue(attribute.Entity, out var entity))
                throw new InvalidOperationException($"{name} refers to {attribute.Entity.FullName}, which is not a searchable entity.");

            var navigation = Navigation.Access(parameter, property);

            var key = navigation.Type.GetProperty(entity.Key.Name, BindingFlags.Public | BindingFlags.Instance);

            if (key == null || key.PropertyType != entity.Key.PropertyType)
                throw new InvalidOperationException($"{name} refers to {entity.Document.Type.Name}, but {navigation.Type.Name} has no key property '{entity.Key.Name}' of type {entity.Key.PropertyType.Name}.");

            if (!keys.TryAdd(entity.Document.Type, CreateKey(entity, false, $"{GetName(property)}.{GetName(key)}", Expression.Property(navigation, key), parameter)))
                throw new InvalidOperationException($"{typeof(TDocument).Name} refers to {entity.Document.Type.Name} more than once.");
        }

        return keys;
    }

    private static DocumentKey<TDocument> CreateKey(SearchEntity entity, bool isIdentity, string name, Expression access, ParameterExpression parameter)
    {
        var key = Expression.Lambda(access, parameter);
        var keyType = typeof(DocumentKey<,>).MakeGenericType(typeof(TDocument), access.Type);

        return (DocumentKey<TDocument>)Activator.CreateInstance(keyType, entity, isIdentity, name, key);
    }

    private static string GetName(PropertyInfo property)
    {
        return JsonNamingPolicy.CamelCase.ConvertName(property.Name);
    }
}
