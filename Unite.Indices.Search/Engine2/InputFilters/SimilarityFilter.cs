namespace Unite.Indices.Search.Engine2.InputFilters;

/// <summary>
/// Full-text match of a string property against any of the values.
/// </summary>
public class SimilarityFilter : FilterNode
{
    public List<string> Values { get; set; }
}
