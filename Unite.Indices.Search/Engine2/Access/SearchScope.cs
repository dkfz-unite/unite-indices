namespace Unite.Indices.Search.Engine2.Access;

/// <summary>
/// Data a search may use: all data (root users, internal use) or the data a user has access to.
/// </summary>
public sealed class SearchScope
{
    /// <summary>
    /// All data, without restriction (root users, internal use).
    /// </summary>
    public static SearchScope All { get; } = new(null, true);

    /// <summary>
    /// Data the user has access to; without a user (anonymous) nothing is accessible.
    /// </summary>
    public static SearchScope ForUser(int? userId) => new(userId, false);

    public int? UserId { get; }
    public bool IsAll { get; }


    private SearchScope(int? userId, bool isAll)
    {
        UserId = userId;
        IsAll = isAll;
    }
}
