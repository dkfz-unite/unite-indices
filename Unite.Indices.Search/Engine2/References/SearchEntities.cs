using System.Reflection;
using Unite.Indices.Entities;
using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.References;

/// <summary>
/// Searchable entities: searchable documents with a key (<see cref="SearchableDocumentAttribute.Key"/>).
/// </summary>
public static class SearchEntities
{
    private static readonly Lazy<IReadOnlyDictionary<Type, SearchEntity>> _entities = new(Build);

    public static IReadOnlyDictionary<Type, SearchEntity> All => _entities.Value;


    private static IReadOnlyDictionary<Type, SearchEntity> Build()
    {
        var entities = new Dictionary<Type, SearchEntity>();

        foreach (var document in SearchDocuments.All)
        {
            var attribute = document.Type.GetCustomAttribute<SearchableDocumentAttribute>(false);

            if (attribute.Key == null)
            {
                if (attribute.Level != 0)
                    throw new InvalidOperationException($"Document {document.Type.FullName} has an entity level, but no key.");

                continue;
            }

            if (attribute.Level <= 0)
                throw new InvalidOperationException($"Entity {document.Type.FullName} needs a positive level.");

            var key = document.Type.GetProperty(attribute.Key, BindingFlags.Public | BindingFlags.Instance)
                ?? throw new InvalidOperationException($"Key property '{attribute.Key}' of entity {document.Type.FullName} does not exist.");

            var sameLevel = entities.Values.FirstOrDefault(entity => entity.Level == attribute.Level);

            if (sameLevel != null)
                throw new InvalidOperationException($"Entities {sameLevel.Document.Type.FullName} and {document.Type.FullName} have the same level {attribute.Level}.");

            entities.Add(document.Type, new SearchEntity(document, key, attribute.Level));
        }

        return entities;
    }
}

/// <summary>
/// Searchable entity with its key property and level (higher is finer).
/// </summary>
public sealed class SearchEntity(SearchDocument document, PropertyInfo key, int level)
{
    public SearchDocument Document { get; } = document;
    public PropertyInfo Key { get; } = key;
    public int Level { get; } = level;
}
