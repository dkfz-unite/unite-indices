using Unite.Indices.Search.Engine.Filters;

namespace Unite.Indices.Search.Engine2.InputFilters;

public class FilterGroup : FilterNode
{
    public LogicalOperator Operator { get; set; } = LogicalOperator.And;
    public List<FilterNode> FilterNodes { get; set; }
}
