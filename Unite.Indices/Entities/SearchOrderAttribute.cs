namespace Unite.Indices.Entities;

/// <summary>
/// Sort order of search results of a searchable document: the property path (e.g. "stats.donors"), direction and priority.
/// Several orders are applied by ascending priority.
/// </summary>
/// <param name="path">Dot-separated, camel-cased path of the property (e.g. "stats.donors.number").</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class SearchOrderAttribute(string path) : Attribute
{
    public string Path { get; } = path;

    /// <summary>
    /// Sort descending (e.g. most donors first).
    /// </summary>
    public bool Descending { get; set; }

    /// <summary>
    /// Order of application, lower first.
    /// </summary>
    public int Priority { get; set; }
}
