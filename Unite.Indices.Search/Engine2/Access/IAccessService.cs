using Unite.Indices.Search.Engine2.Criteria;

namespace Unite.Indices.Search.Engine2.Access;

/// <summary>
/// Restricts searches of users to the data they have access to.
/// </summary>
public interface IAccessService
{
    /// <summary>
    /// Restriction of a search of the target in the scope, null if the search is not restricted.
    /// </summary>
    /// <param name="scope">Scope of the search (not <see cref="SearchScope.All"/>).</param>
    /// <param name="targetType">Document type searched.</param>
    /// <param name="hasDocumentCriteria">The search has criteria on other documents than the target.</param>
    Task<AccessRestriction> GetRestriction(SearchScope scope, Type targetType, bool hasDocumentCriteria);

    /// <summary>
    /// Adds access information to found documents (e.g. whether projects are accessible to the user).
    /// </summary>
    Task Annotate<T>(IEnumerable<T> documents, SearchScope scope) where T : class;
}

/// <summary>
/// Mandatory include criteria on a document restricting a search (e.g. donors of accessible projects).
/// </summary>
public sealed class AccessRestriction
{
    /// <summary>
    /// Nothing is accessible: the search finds nothing.
    /// </summary>
    public static AccessRestriction Nothing { get; } = new(null, null);

    public Type DocumentType { get; }
    public CriteriaGroup Criteria { get; }
    public bool IsNothing => DocumentType == null;


    public AccessRestriction(Type documentType, CriteriaGroup criteria)
    {
        DocumentType = documentType;
        Criteria = criteria;
    }
}
