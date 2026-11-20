using Unite.Indices.Search.Engine2.Fields;

namespace Unite.Indices.Search.Engine2.References;

/// <summary>
/// Entity hierarchies (trees) and the execution rank of search stages derived from them.
/// The main tree is the one the other trees refer to (donors -> specimens); every other tree is attached to the
/// finest main-tree entity its entities refer to (genes -> proteins at specimens).
/// </summary>
public static class SearchTrees
{
    private static readonly Lazy<(SearchDocument Main, IReadOnlyDictionary<SearchDocument, int> Attachments)> _trees = new(Build);

    /// <summary>
    /// Root of the main tree.
    /// </summary>
    public static SearchDocument Main => _trees.Value.Main;

    /// <summary>
    /// Level of the main-tree entity each other tree (by root) is attached to.
    /// </summary>
    public static IReadOnlyDictionary<SearchDocument, int> Attachments => _trees.Value.Attachments;


    /// <summary>
    /// Execution rank of a document stage, compared in order:
    /// main-tree level; main-tree entities (0), then entities of attached trees (1), then other documents (2); level in the own tree.
    /// E.g. donors (1,0,1), projects (1,2,0), specimens (2,0,2), genes (2,1,1), proteins (2,1,2), gene_expressions (2,2,0).
    /// </summary>
    public static (int Level, int Group, int Depth) GetRank(IDocumentStage stage)
    {
        var identity = stage.Keys.FirstOrDefault(key => key.IsIdentity);

        if (identity != null)
        {
            return identity.Entity.Root == Main
                ? (identity.Entity.Level, 0, identity.Entity.Level)
                : (Attachments[identity.Entity.Root], 1, identity.Entity.Level);
        }

        var level = stage.Keys.Select(key => key.Entity.Root == Main ? key.Entity.Level : Attachments[key.Entity.Root]).DefaultIfEmpty(0).Max();

        return (level, 2, 0);
    }


    private static (SearchDocument, IReadOnlyDictionary<SearchDocument, int>) Build()
    {
        var entities = SearchEntities.All.Values.ToList();
        var roots = entities.Select(entity => entity.Root).Distinct().ToList();

        // References of each tree's entities to entities of other trees.
        var references = roots.ToDictionary(root => root, root => entities
            .Where(entity => entity.Root == root)
            .SelectMany(entity => DocumentStages.Get(entity.Document.Type).Keys)
            .Where(key => key.Entity.Root != root)
            .ToList());

        var mains = roots.Where(root => references[root].Count == 0).ToList();

        if (roots.Count > 0 && mains.Count != 1)
            throw new InvalidOperationException(mains.Count == 0
                ? "Entity trees refer to each other: there is no main tree that refers to no other tree."
                : $"Several entity trees refer to no other tree ({string.Join(", ", mains.Select(root => root.Name))}): the main tree is ambiguous.");

        var main = mains.SingleOrDefault();
        var attachments = new Dictionary<SearchDocument, int>();

        foreach (var root in roots.Where(root => root != main))
        {
            var mainKeys = references[root].Where(key => key.Entity.Root == main).ToList();

            if (mainKeys.Count == 0)
                throw new InvalidOperationException($"Entity tree '{root.Name}' refers to no entity of the main tree '{main.Name}'.");

            attachments.Add(root, mainKeys.Max(key => key.Entity.Level));
        }

        return (main, attachments);
    }
}
