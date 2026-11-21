using System.Reflection;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.References;

/// <summary>
/// Searchable entities: searchable documents with a key property (<see cref="EntityKeyAttribute"/>).
/// Their hierarchies are derived from references: a single-object reference to another entity points to the parent,
/// entities without a parent are roots (e.g. donors -> specimens, genes -> proteins).
/// </summary>
public static class SearchEntities
{
    private static readonly Lazy<IReadOnlyDictionary<Type, SearchEntity>> _entities = new(Build);

    public static IReadOnlyDictionary<Type, SearchEntity> All => _entities.Value;


    private static IReadOnlyDictionary<Type, SearchEntity> Build()
    {
        var keys = new Dictionary<Type, (SearchDocument Document, PropertyInfo Key)>();

        foreach (var document in SearchDocuments.All)
        {
            var key = FindKey(document.Type);

            if (key != null)
                keys.Add(document.Type, (document, key));
        }

        var parents = keys.Keys.ToDictionary(type => type, type => GetParent(type, keys.Keys));

        var entities = new Dictionary<Type, SearchEntity>();

        foreach (var type in keys.Keys)
        {
            var path = GetPath(type, parents);

            entities.Add(type, new SearchEntity(keys[type].Document, keys[type].Key, path.Count, keys[path[^1]].Document, parents[type]));
        }

        return entities;
    }

    /// <summary>
    /// Property marked with <see cref="EntityKeyAttribute"/>, or null if the type has none.
    /// </summary>
    internal static PropertyInfo FindKey(Type type)
    {
        var keys = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.IsDefined(typeof(EntityKeyAttribute), true))
            .ToList();

        if (keys.Count > 1)
            throw new InvalidOperationException($"{type.FullName} has several entity key properties ({string.Join(", ", keys.Select(key => key.Name))}).");

        return keys.SingleOrDefault();
    }

    /// <summary>
    /// Parent entity: the entity the document refers to with a single object (not a collection), or null for the root.
    /// </summary>
    private static Type GetParent(Type type, IEnumerable<Type> entityTypes)
    {
        var parents = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(property => (Property: property, Attribute: property.GetCustomAttribute<ReferencesAttribute>(true)))
            .Where(entry => entry.Attribute != null)
            .Where(entry => entityTypes.Contains(entry.Attribute.Entity) && entry.Attribute.Entity != type)
            .Where(entry => Navigation.GetElementType(entry.Property.PropertyType) == null)
            .ToList();

        if (parents.Count > 1)
            throw new InvalidOperationException($"Entity {type.FullName} refers to several parent entities with single objects " +
                $"({string.Join(", ", parents.Select(parent => parent.Property.Name))}): an entity can have only one parent.");

        return parents.SingleOrDefault().Attribute?.Entity;
    }

    /// <summary>
    /// Path from the entity up to the root of its hierarchy (entity first, root last).
    /// </summary>
    private static List<Type> GetPath(Type type, IReadOnlyDictionary<Type, Type> parents)
    {
        var path = new List<Type>();

        for (var current = type; current != null; current = parents[current])
        {
            if (path.Contains(current))
                throw new InvalidOperationException($"Parent references of entity {type.FullName} form a cycle.");

            path.Add(current);
        }

        return path;
    }
}

/// <summary>
/// Searchable entity with its key property, level in its hierarchy (1 for the root, higher is finer) and the root of the hierarchy.
/// </summary>
public sealed class SearchEntity(SearchDocument document, PropertyInfo key, int level, SearchDocument root, Type parent)
{
    public SearchDocument Document { get; } = document;
    public PropertyInfo Key { get; } = key;
    public int Level { get; } = level;
    public SearchDocument Root { get; } = root;

    /// <summary>
    /// Document type of the parent entity, null for a root.
    /// </summary>
    public Type Parent { get; } = parent;
}
