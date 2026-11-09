namespace Unite.Indices.Search.Engine2.InputFilters;

/// <summary>
/// Numeric range filter (inclusive). Either bound can be omitted.
/// </summary>
public class RangeFilter : FilterNode
{
    public double? From { get; set; }
    public double? To { get; set; }
}
