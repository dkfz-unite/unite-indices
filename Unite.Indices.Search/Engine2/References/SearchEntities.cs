using System.Reflection;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.References;

/// <summary>
/// Searchable entities: searchable documents with a key property (<see cref="EntityKeyAttribute"/>).
/// Their hierarchy is derived from references: a single-object reference to another entity points to the parent.
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

        var roots = parents.Where(entry => entry.Value == null).Select(entry => entry.Key).ToList();

        if (keys.Count > 0 && roots.Count != 1)
            throw new InvalidOperationException(roots.Count == 0
                ? "Searchable entities have no root: their parent references form a cycle."
                : $"Searchable entities have several roots ({string.Join(", ", roots.Select(root => root.FullName))}): " +
                  "all but one must refer to their parent entity with a single-object property marked with [References].");

        var entities = new Dictionary<Type, SearchEntity>();

        foreach (var type in keys.Keys)
            entities.Add(type, new SearchEntity(keys[type].Document, keys[type].Key, GetLevel(type, parents)));

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
    /// Depth of the entity in the hierarchy: 1 for the root, then 2, 3, ...
    /// </summary>
    private static int GetLevel(Type type, IReadOnlyDictionary<Type, Type> parents)
    {
        var path = new HashSet<Type>();

        for (var current = type; current != null; current = parents[current])
        {
            if (!path.Add(current))
                throw new InvalidOperationException($"Parent references of entity {type.FullName} form a cycle.");
        }

        return path.Count;
    }
}

/// <summary>
/// Searchable entity with its key property and level in the entity hierarchy (1 for the root, higher is finer).
/// </summary>
public sealed class SearchEntity(SearchDocument document, PropertyInfo key, int level)
{
    public SearchDocument Document { get; } = document;
    public PropertyInfo Key { get; } = key;
    public int Level { get; } = level;
}
